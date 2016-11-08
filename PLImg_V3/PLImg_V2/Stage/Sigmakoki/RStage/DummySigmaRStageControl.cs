using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLImg_V2
{
    public class DummySigmaRStageControl : ISigmaKokiStage.ISigmakokiStageUnit
    {
        public string ForceStopCommand
        {
            get; set;
        }

        public string MovePosSetCommand
        {
            get; set;
        }

        public string MoveSettedPosCommand
        {
            get; set;
        }

        public string OriginCommand
        {
            get; set;
        }

        public string StatusCommand
        {
            get; set;
        }

        public void ForceStop( )
        {
            Console.WriteLine( "Dummy input Accepted" );
        }

        public void MoveAbsPos( int pos )
        {
            Console.WriteLine( "Dummy input Accepted" );
        }

        public void Origin( )
        {
            Console.WriteLine( "Dummy input Accepted" );
            Console.WriteLine( "Dummy input Accepted" );
        }

        public string Query( string command )
        {
            Console.WriteLine( "Dummy input Accepted" );
            return "";
        }

        public string Read( )
        {
            Console.WriteLine( "Dummy input Accepted" );
            return "";
        }

        public bool RStageConnect( string port )
        {
            Console.WriteLine( "Dummy input Accepted" );
            return true;
        }

        public bool RstageRelease( )
        {
            Console.WriteLine( "Dummy input Accepted" );
            return true;
        }

        public void SetRSpeed( int speed, int acc )
        {
            Console.WriteLine( "Dummy input Accepted" );
        }

        public string Status( )
        {
            Console.WriteLine( "Dummy input Accepted" );
            return "";
        }

        public void Wait2Arrive( int targetPos )
        {
            Console.WriteLine( "Dummy input Accepted" );
        }

        public void Wait2ArriveEpsilon( int targetPos, double epsilon )
        {
            Console.WriteLine( "Dummy input Accepted" );
        }

        public void Write( string command )
        {
            Console.WriteLine( "Dummy input Accepted" );
        }
    }
}

