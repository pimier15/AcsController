using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLImg_V2
{
    class SerialCom
    {
        const int TransSpeed = 38400;
        private RS232C mbSession;

        public void OpenSession( String Comport )
        {
            try
            {
                mbSession = new RS232C();
                mbSession.Connect( Comport , TransSpeed );
                mbSession.Delimiter = new byte[] { 0x0d, 0x0a };
            }
            catch ( Exception e )
            {
                throw e;
            }
        }

        public void CloseSession( )
        {
            mbSession.Release();
        }

        public void WriteCommand( String command )
        {
            string cmd = null;
            cmd = command;
            mbSession.Write( cmd );
        }

        public string Query( string str )
        {
            string buffer = null;

            mbSession.ReadTimeout = 8000;
            buffer = mbSession.Query( str );

            return buffer;
        }

        public void Write( string str ) {
            mbSession.Write( str );
        }

        internal void SetTimeOut( int p )
        {
            mbSession.ReadTimeout = p;
        }
    }
}
