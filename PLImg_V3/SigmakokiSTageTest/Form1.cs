using NationalInstruments.VisaNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmakokiSTageTest
{
    public partial class Form1 : Form
    {
        public Form1( ) {
            InitializeComponent();
        }


        MessageBasedSession connect;
        //SerialCom connect;

        public string MovePosSetCommand { get { return "M:1{0}P{1}"; } set {; } }
        public string MoveSettedPosCommand { get { return "G:"; } set {; } }
        public string OriginCommand { get { return "H:1"; } set {; } }
        public string StatusCommand { get { return "!:"; } set {; } }
        public string ForceStopCommand { get { return "L: 1"; } set {; } }

        public void ForceStop( ) {
            Write( ForceStopCommand );
        }

        public void MoveAbsPos( int pos ) {
            Write( String.Format( MovePosSetCommand, pos > 0 ? "+" : "-", (int)Math.Abs( pos ) ) );
            Write( MoveSettedPosCommand );
        }

        public void Origin( ) {
            Write( OriginCommand );
        }

     

        public bool RStageConnect( string port ) {
            try
            {
                connect = (MessageBasedSession)ResourceManager.GetLocalManager().Open( "COM" + port );
                connect.SetAttributeInt32( NationalInstruments.VisaNS.AttributeType.AsrlBaud, 38400 );
                connect.Timeout = 1000;
                Thread.Sleep( 100 );
                connect.Dispose();
                connect = (MessageBasedSession)ResourceManager.GetLocalManager().Open( "COM" + port );
                connect.SetAttributeInt32( NationalInstruments.VisaNS.AttributeType.AsrlBaud, 38400 );
                connect.Timeout = 1000;
                //connect = new SerialCom();
                //connect.OpenSession( port );
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }

        public bool RstageRelease( ) {
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

        public void SetRSpeed( int speed, int acc ) {
            string input = String.Format("D:1S{0}F{1}R{2}", speed, speed, acc);
            Write( input );
        }

        public string Status( ) {
            return Query( StatusCommand );
        }

        #region OrderCommand
        public string Query( string command ) {
            return connect.Query( command + "\r\n" );
        }

        public string Read( ) {
            return "not supported";
        }

        public void Write( string command ) {
            connect.Write( command + "\r\n" );
        }
        #endregion

        #region sub
        void CheckPosition( ) {
            string currentstatus;
            do
            {
                Thread.Sleep( 50 );
                currentstatus = Query( StatusCommand );
                Console.WriteLine( currentstatus.ToString() );
            }
            while ( currentstatus != "R\r\n" );
            
            //string[] tempo = currentstatus.Split(',');
            //return tempo[0].Replace( " ", string.Empty );
        }
        #endregion

        private void btnConnect_Click( object sender, EventArgs e ) {
            RStageConnect( txbPort.Text );
        }

        private void btnorigin_Click( object sender, EventArgs e ) {
            Origin();
        }

        private void btnsetMove_Click( object sender, EventArgs e ) {
            Write( String.Format( MovePosSetCommand, Convert.ToInt32(txbMovePos.Text) > 0 ? "+" : "-", (int)Math.Abs( Convert.ToInt32( txbMovePos.Text ) ) ) );
            
        }

        private void btnGo_Click( object sender, EventArgs e ) {
            Write( MoveSettedPosCommand );
            CheckPosition();
        }

        private void btnGetStatus_Click( object sender, EventArgs e ) {
            string output =  Query("!:");
            lblStatus.Text = output;
        }

        private void f( object sender, EventArgs e ) {

        }

        private void Form1_FormClosing( object sender, FormClosingEventArgs e ) {
            Dispose();
        }

        private void btnDisconnect_Click( object sender, EventArgs e ) {
            Dispose();
        }
    }
}
