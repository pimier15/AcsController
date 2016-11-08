using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace PLImg_V2
{
    public class RS232C
    {
        private SerialPort Serial;
        public bool IsConnected { get; set; }
        public string DeviceName { get { return "RS323C"; } }
        public string LastErrorMsg { get; set; }
        public byte[] Delimiter { get; set; }
        public int Port { get; set; }
        public int BaudRate { get; set; }
        public int ReadTimeout { get; set; }

        public RS232C( )
        {
        }

        public bool Connect( string connectArgs, int transspeed )
        {
            string[] str = connectArgs.Split(',');
            if ( str.Length == 1 )
            {
                return Open( Convert.ToInt32( str[0] ), transspeed );
            }
            else if ( str.Length == 2 )
            {
                return Open( Convert.ToInt32( str[0] ), Convert.ToInt32( str[1] ) );
            }
            else if ( str.Length == 6 )
            {
                Parity parity = Parity.None;
                switch ( str[2].ToLower() )
                {
                    case "none":
                        parity = Parity.None;
                        break;
                    case "even":
                        parity = Parity.Even;
                        break;
                    case "odd":
                        parity = Parity.Odd;
                        break;
                    default:
                        parity = Parity.None;
                        break;
                }
                StopBits stopbits = StopBits.One;
                switch ( str[4].ToLower() )
                {
                    case "one":
                        stopbits = StopBits.One;
                        break;
                    case "two":
                        stopbits = StopBits.Two;
                        break;
                    case "none":
                        stopbits = StopBits.None;
                        break;
                    default:
                        stopbits = StopBits.One;
                        break;
                }
                return Open( Convert.ToInt32( str[0] ), Convert.ToInt32( str[1] ), parity, Convert.ToInt32( str[3] ), stopbits, Convert.ToInt32( str[5] ) );
            }
            else
            {
                return false;
            }
        }

        public bool Release( )
        {
            return Close();
        }




        // method
        /// <summary>
        /// open
        /// </summary>
        /// <param name="port"></param>
        /// <param name="baudrate"></param>
        /// <param name="parity"></param>
        /// <param name="databits"></param>
        /// <param name="stopbits"></param>
        /// <returns></returns>
        public bool Open( int port, int baudrate, Parity parity, int databits, StopBits stopbits, int readTimeout )
        {
            Port = port;
            BaudRate = baudrate;
            ReadTimeout = readTimeout;

            // var
            string comport = string.Format("COM{0}", port);

            // create
            Serial = new SerialPort( comport, baudrate, parity, databits, stopbits );
            Serial.Handshake = Handshake.None;
            Serial.DtrEnable = true;
            Serial.RtsEnable = true;
            Serial.DiscardNull = false;
            Serial.ParityReplace = 63;
            Serial.ReadBufferSize = 16384;
            Serial.ReadTimeout = readTimeout;
            Serial.ReceivedBytesThreshold = 1;
            Serial.WriteBufferSize = 16384;
            Serial.WriteTimeout = -1;

            try
            {
                Serial.Open();
            }
            catch ( Exception e )
            {
                Console.Write( e.ToString() );
                return false;
            }

            IsConnected = Serial.IsOpen;

            return IsConnected;
        }


        /// <summary>
        /// open simple
        /// </summary>
        /// <param name="port"></param>
        /// <param name="baudrate"></param>
        /// <returns></returns>
        public bool Open( int port, int baudrate )
        {
            return Open( port, baudrate, Parity.None, 8, StopBits.One, 10 * 1000 );
        }



        /// <summary>
        /// close
        /// </summary>
        /// <returns></returns>
        public bool Close( )
        {
            if ( IsConnected )
            {
                IsConnected = false;
                Serial.Close();
                return true;
            }

            return false;
        }



        /// <summary>
        /// query
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string Query( string req )
        {
            // check req string
            if ( req == null || req.Length == 0 )
            {
                return "";
            }

            // check
            if ( IsConnected )
            {
                lock ( Serial )
                {
                    try
                    {
                        var arr = System.Text.Encoding.ASCII.GetBytes(req);
                        Serial.Write( arr, 0, arr.Length );
                        Serial.Write( Delimiter, 0, Delimiter.Length );


                        List<byte> recvPacket = new List<byte>();

                        Thread.Sleep( 1 );

                        DateTime TimeoutTime = DateTime.Now.AddMilliseconds(ReadTimeout);
                        while ( TimeoutTime > DateTime.Now )
                        {
                            if ( Serial.BytesToRead > 0 )
                            {
                                byte[] readbyte = new byte[Serial.BytesToRead];
                                Serial.Read( readbyte, 0, readbyte.Length );
                                recvPacket.AddRange( readbyte );
                            }

                            if ( CheckDelimiter( recvPacket ) )
                            {
                                break;
                            }
                        }
                        if ( recvPacket.Count > Delimiter.Length )
                        {
                            return Encoding.ASCII.GetString( recvPacket.ToArray(), 0, recvPacket.Count - Delimiter.Length );
                        }
                        else
                        {
                            return "";
                        }
                    }
                    catch ( ArgumentOutOfRangeException e )
                    {
                        Console.WriteLine( "error = " + e.Message );
                    }
                }
            }

            // empty
            return "";
        }


        /// <summary>
        /// query
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public void Write( string req )
        {
            // check
            if ( IsConnected )
            {
                lock ( Serial )
                {
                    try
                    {
                        var arr = System.Text.Encoding.ASCII.GetBytes(req);
                        Serial.Write( arr, 0, arr.Length );
                        Serial.Write( Delimiter, 0, Delimiter.Length );
                    }
                    catch ( ArgumentOutOfRangeException e )
                    {
                        Console.WriteLine( "error = " + e.Message );
                    }
                }
            }

        }

        private bool CheckDelimiter( List<byte> recvPacket )
        {
            if ( recvPacket.Count >= Delimiter.Length )
            {
                for ( int i = 0; i < Delimiter.Length; i++ )
                {
                    if ( recvPacket[recvPacket.Count - i - 1] != Delimiter[Delimiter.Length - i - 1] )
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// send data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count">sended byte length</param>
        /// <returns></returns>
        public int WriteData( byte[] data, int offset, int count )
        {
            if ( Serial.IsOpen )
            {
                Serial.Write( data, offset, count );
                return count;
            }

            return 0;
        }

        public int WriteData( char[] data, int offset, int count )
        {
            if ( Serial.IsOpen )
            {
                Serial.Write( data, offset, count );
                return count;
            }

            return 0;
        }



        /// <summary>
        /// get count
        /// </summary>
        /// <returns></returns>
        public int BytesToRead( )
        {
            if ( Serial.IsOpen )
            {
                return Serial.BytesToRead;
            }

            return 0;
        }



        /// <summary>
        /// read data
        /// </summary>
        /// <param name="ref_data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int ReadData( ref byte[] ref_data, int offset, int count )
        {
            if ( Serial.IsOpen )
            {
                int nRead = Serial.Read(ref_data, offset, count);
                return nRead;
            }

            return 0;
        }


        /// <summary>
        /// read 1 byte
        /// </summary>
        /// <returns>if -1 then error</returns>
        public int ReadByte( )
        {
            if ( Serial.IsOpen )
            {
                int nRead = 0;
                try
                {
                    nRead = Serial.ReadByte();
                }
                catch ( TimeoutException e )
                {
                    Console.WriteLine( "error = " + e.Message );
                    return 0;
                }

                return nRead;
            }

            return 0;
        }



        /// <summary>
        /// read short
        /// </summary>
        /// <returns></returns>
        public int ReadShort( )
        {
            if ( Serial.IsOpen )
            {
                int nRead0 = Serial.ReadByte();
                int nRead1 = Serial.ReadByte();
                short sh = (short)((nRead0 & 0xFF) | ((nRead1 & 0xFF) << 8));
                return sh;
            }

            return 0;
        }


        /// <summary>
        /// read unsigned short
        /// </summary>
        /// <returns></returns>
        public int ReadUShort( )
        {
            if ( Serial.IsOpen )
            {
                int nRead0 = Serial.ReadByte();
                int nRead1 = Serial.ReadByte();
                ushort sh = (ushort)((nRead0 & 0xFF) | ((nRead1 & 0xFF) << 8));
                return sh;
            }

            return 0;
        }
    }
}


