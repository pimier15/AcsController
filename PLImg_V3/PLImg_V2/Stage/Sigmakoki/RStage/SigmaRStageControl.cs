using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;
using System.Threading;

namespace PLImg_V2
{
    public class SigmaRStageControl : ISigmaKokiStage.ISigmakokiStageUnit
    {
        MessageBasedSession connect; 
        //SerialCom connect;

        public string MovePosSetCommand { get { return "A:1{0}P{1}"; } set {; } }
        public string MoveSettedPosCommand { get { return "G:"; } set {; } }
        public string OriginCommand { get { return "H:1"; } set {; } }
        public string StatusCommand { get { return "Q:"; } set {; } }
        public string ForceStopCommand { get { return "L: 1"; } set {; } }

        public void ForceStop( )
        {
            Write( ForceStopCommand );
        }

        public void MoveAbsPos( int pos )
        {
            Write( String.Format( MovePosSetCommand, pos > 0 ? "+" : "-", (int)Math.Abs( pos ) ) );
            Thread.Sleep( 500 );
            Write( MoveSettedPosCommand );
        }

        public void Origin( )
        {
            Write( OriginCommand);
        }

        
        public bool RStageConnect( string port )
        {
            try
            {
                connect = (MessageBasedSession)ResourceManager.GetLocalManager().Open( "COM" + port );
                connect.SetAttributeInt32( NationalInstruments.VisaNS.AttributeType.AsrlBaud, 38400 );
                connect.Timeout = 1000;
                Thread.Sleep( 100 );
                
                //connect = new SerialCom();
                //connect.OpenSession( port );
                return true;
            }
            catch ( Exception ex)
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }

        public bool RstageRelease( )
        {
            try
            {
                //connect.CloseSession();
                connect.Dispose();
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }

        public void SetRSpeed( int speed, int acc )
        {
            string input = String.Format("D:1S{0}F{1}R{2}", speed, speed, acc);
            Write( input );
        }

        public string Status( )
        {
            return Query( StatusCommand );
        }

        public void Wait2Arrive( int targetPos )
        {
            
        }

        public void Wait2ArriveEpsilon( int targetPos, double epsilon )
        {
            
        }

        public void GO( ) {
            Write( MoveSettedPosCommand );
        }


        #region OrderCommand
        public string Query( string command )
        {
            return connect.Query( command + "\r\n" );
        }

        public string Read( )
        {
            return "not supported";
        }

        public void Write( string command )
        {
            connect.Write( command + "\r\n" );
        }
        #endregion

        #region sub
        bool IsReady( ) {
            if ( Query( StatusCommand ).TrimEnd( '\r', '\n' ) == "R" )
            { return true; }
            else
            { return false; }
        }

        #endregion

    }
}
