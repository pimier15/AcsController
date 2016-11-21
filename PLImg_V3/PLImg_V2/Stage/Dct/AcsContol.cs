using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPIIPLUSCOM660Lib;
using System.Runtime.InteropServices;

namespace PLImg_V2
{
    public class AcsContol
    {
        object pWait = 0;
        Dictionary<string,int> Axis;
        SPIIPLUSCOM660Lib.AsyncChannel Ch;

        public AcsContol( )
        {
            Ch = new SPIIPLUSCOM660Lib.AsyncChannel();
            Axis = new Dictionary<string, int>();
            Axis.Add( "X", 0 );
            Axis.Add( "Y", 1 );
            Axis.Add( "Z", 2 );
        }

        public void Connect(string addIP )
        {
            try
            {
                Ch.OpenCommEthernetTCP( addIP , Ch.ACSC_SOCKET_STREAM_PORT);
                EnableMotor();
            }
            catch ( COMException ex )
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ConnectCom( string addIP ) {
            try
            {
                Ch.OpenCommSerial( Convert.ToInt32(addIP), -1 );
                EnableMotor();
            }
            catch ( COMException ex )
            {
                Console.WriteLine( ex.ToString() );
            }
        }

        public void EnableMotor( )
        {
            Ch.Enable( Axis["X"], Ch.ACSC_ASYNCHRONOUS, ref pWait );
            Ch.Enable( Axis["Y"], Ch.ACSC_ASYNCHRONOUS, ref pWait );
            //Ch.Enable( Axis["Z"], Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        //public Action<int> DisableMotor(int axisindex ) {

        //    Action<int> Disable = axis =>
        //    {
        //        Ch.Disable(Axis.Values.ElementAt(axisindex),Ch.ACSC_ASYNCHRONOUS, ref pWait);
        //    };
        //    return Disable;
        //}

        public void DisableMotor( int axisindex ) {
                Ch.Disable(Axis.Values.ElementAt(axisindex),Ch.ACSC_ASYNCHRONOUS, ref pWait);
        }

        public void DisZ( ) {
            Ch.Disable( 2, Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        public void Home( )
        {
            Ch.RunBuffer(0, "", Ch.ACSC_ASYNCHRONOUS, ref pWait );
            Ch.RunBuffer(1, "", Ch.ACSC_ASYNCHRONOUS, ref pWait );
            Ch.RunBuffer(2, "", Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        public void BuffClear( ) {
            Ch.ClearBuffer( Axis["X"] ,1,Ch.ACSC_MAX_LINE, Ch.ACSC_ASYNCHRONOUS, ref pWait );
            Ch.ClearBuffer( Axis["Y"] ,1,Ch.ACSC_MAX_LINE, Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        public void XMove( int pos )
        {
            var xmove = FcMove(Axis["X"]);
            xmove( pos );
        }

        public void YMove( int pos )
        {
            var ymove = FcMove(Axis["Y"]);
            ymove( pos );
        }

        public void ZMove( int pos )
        {
            var zmove = FcMove(Axis["Z"]);
            zmove( pos );
        }

        public void Halt( ) {
            for ( int i = 0; i < 3; i++ )
            {
                Ch.Halt( Axis.Values.ElementAt( i ), Ch.ACSC_ASYNCHRONOUS, ref pWait );
            }
        }

        public double[] GetMotorFPos( )
        {
            try
            {
                double[] output = new double[3];

                for ( int i = 0; i < 3; i++ )
                {
                    output[i] = Ch.GetFPosition( Axis.Values.ElementAt( i ), Ch.ACSC_SYNCHRONOUS, ref pWait );
                }
                return output;

            }
            catch ( Exception )
            {
                double[] temp = new double[3] { 0,0,0 };
                return temp;
            }
        }

        public void SetSpeed( int xspeed, int yspeed, int zspeed )
        {
            Ch.SetVelocity( Axis["X"], xspeed,Ch.ACSC_ASYNCHRONOUS,  ref pWait  );
            Ch.SetVelocity( Axis["Y"], yspeed, Ch.ACSC_ASYNCHRONOUS, ref pWait  );
        }

        public void SetXSpeed( int xspeed )
        {
            FcSetSpeed( Axis["X"] )( xspeed );
        }

        public void SetYSpeed( int yspeed )
        {
            FcSetSpeed(Axis["Y"])(yspeed);
        }

        public void SetZSpeed( int zspeed )
        {
            FcSetSpeed(Axis["Z"])(zspeed);
        }

        public void Wait2ArriveEpsilon( string axis, double targetPos, double epsilon )
        {
            while(true)
            {
                double error = Math.Abs( targetPos - Ch.GetFPosition( Axis[axis], Ch.ACSC_SYNCHRONOUS, ref pWait ) );
                if ( error < epsilon ) break;
            }
        }

        public void Dispose( ) {
            Ch.CloseComm();
        }

        #region func
        Action<int> FcSetSpeed( int axis )
        {
            Action<int> setspeed = speed =>
            {
                Ch.SetVelocity( axis, speed, Ch.ACSC_ASYNCHRONOUS, ref pWait );
            };
            return setspeed;
        }

        Action<double> FcMove( int axis )
        {
            Action<double> move = point =>
            {
                Ch.ToPoint( 0,axis, point,Ch.ACSC_ASYNCHRONOUS,  ref pWait  );
            };
            return move;
        }
        #endregion




    }
}
