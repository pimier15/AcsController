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

        public void Connect(int port )
        {
            try
            {
                Ch.OpenCommSerial( port, -1 );
                EnableMotor();
            }
            catch ( COMException ex )
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void EnableMotor( )
        {
            Ch.Enable( Axis["X"], Ch.ACSC_ASYNCHRONOUS, ref pWait );
            Ch.Enable( Axis["Y"], Ch.ACSC_ASYNCHRONOUS, ref pWait );
            //Ch.Enable( Axis["Z"], Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        public void XMove( int pos )
        {
            Ch.ToPoint( Ch.ACSC_AMF_RELATIVE, Axis["X"], pos, Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        public void YMove( int pos )
        {
            Ch.ToPoint( Ch.ACSC_AMF_RELATIVE, Axis["Y"], pos, Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        public void ZMove( int pos )
        {
            Ch.ToPoint( Ch.ACSC_AMF_RELATIVE, Axis["Z"], pos, Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        public double[] GetMotorFPos( )
        {
            double[] output = new double[3];
            
            for ( int i = 0; i < 3; i++ )
            {
                output[i] = Ch.GetFPosition( Axis.Values.ElementAt(i), Ch.ACSC_SYNCHRONOUS, ref pWait);
            }
            return output;
        }

        public void SetSpeed( int xspeed, int yspeed, int zspeed )
        {
            Ch.SetVelocityImm( Axis["X"], xspeed,Ch.ACSC_ASYNCHRONOUS,ref pWait  );
            Ch.SetVelocityImm( Axis["Y"], yspeed, Ch.ACSC_ASYNCHRONOUS, ref pWait  );
            Ch.SetVelocityImm( Axis["Z"], zspeed, Ch.ACSC_ASYNCHRONOUS, ref pWait );
        }

        public double[] FeedbackPos()
        {
            double[] output = new double[3];
            output[0] = Math.Round( Ch.GetFPosition(Axis["X"], Ch.ACSC_SYNCHRONOUS, ref pWait) , 2 );
            output[1] = Math.Round( Ch.GetFPosition(Axis["Y"], Ch.ACSC_SYNCHRONOUS, ref pWait) , 2 );
            //double FeedbackPosZ = Math.Round( Ch.GetFPosition(Axis["Z"], Ch.ACSC_SYNCHRONOUS, ref pWait); , 2 );
            return output;
        }


    }
}
