using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;
using System.Threading;

namespace PLImg_V2
{
    public class DctXYStageControl : IDctXYZStage.IXYStageOrder
    {
        MessageBasedSession Connector;
        public string MovePosSetCommand { get { return "A:1{0}P{1}"; } set {; } }
        public string MoveSettedPosCommand { get { return "G:"; } set {; } }
        public string OriginCommand { get { return "H:1"; } set {; } }
        public string StatusCommand { get { return "Q:"; } set {; } }
        public string ForceStopCommand { get { return "L:E"; } set {; } }



        public void XYForceStop()
        {
            
        }

        public void XYMoveAbsPos(int posX, int posY)
        {
            
        }

        public void XYOrigin()
        {
            
        }

        public void XYSetSpeed(int speedX, int speedY, int accX, int accY)
        {
            
        }

        public bool XYStageConnect(string port)
        {
            return false;
        }

        public bool XYStageRelease()
        {
            return true;
        }

        public string XYStatus()
        {
            return "OK";
        }

        public void XYWait2Arrive(int targetPosX, int targetPosY)
        {
            Thread.Sleep( 2000 );
        }

        public void XYWait2ArriveEpsilon(int targetPos, int targetPosY, double epsilon)
        {
            
        }

        #region Sub
        string CheckPosition()
        {
            string currentstatus;
            do
            {
                Thread.Sleep(50);
                currentstatus = Query(StatusCommand);
            }
            while (currentstatus == "OK\r\n");
            string[] tempo = currentstatus.Split(',');
            return tempo[0].Replace(" ", string.Empty);
        }

        public void Write(string command)
        {
            Connector.Write(command + "\r\n");
        }

        public string Query(string command)
        {
            return Connector.Query(command + "\r\n");
        }

        public string Read()
        {
            return Connector.ReadString().Replace("\n", "");
        }

        #endregion
    }
}
