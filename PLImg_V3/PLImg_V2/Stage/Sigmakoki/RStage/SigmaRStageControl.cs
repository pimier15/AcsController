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
        //MessageBasedSession connect; 
        SerialCom connect;

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
                //connect = (MessageBasedSession)ResourceManager.GetLocalManager().Open( "COM" + port );
                //connect.SetAttributeInt32( NationalInstruments.VisaNS.AttributeType.AsrlBaud, 38400 );
                //connect.Timeout = 1000;
                connect = new SerialCom();
                connect.OpenSession( port );
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
                connect.CloseSession();
                //connect.Dispose();
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
            string targetstr = targetPos.ToString();
            string checkstr = CheckPosition();
            while ( checkstr != targetstr )
            {
                Thread.Sleep( 50 );
                checkstr = CheckPosition();
            }
        }

        public void Wait2ArriveEpsilon( int targetPos, double epsilon )
        {
            string targetstr = targetPos.ToString();
            string checkstr = CheckPosition();
            while ( Math.Abs( double.Parse( checkstr ) - double.Parse( targetstr ) ) > epsilon )
            {
                Thread.Sleep( 50 );
                checkstr = CheckPosition();
            }
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
        string CheckPosition( )
        {
            string currentstatus;
            do
            {
                Thread.Sleep( 50 );
                currentstatus = Query( StatusCommand );
            }
            while ( currentstatus == "OK\r\n" );
            string[] tempo = currentstatus.Split(',');
            return tempo[0].Replace( " ", string.Empty );
        }
        #endregion

    }
}
