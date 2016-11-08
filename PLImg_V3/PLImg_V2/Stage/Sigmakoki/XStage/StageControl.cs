using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NationalInstruments.VisaNS;
using System.Threading;

namespace PLImg_V2
{
    public class StageControl 
    { 
        MessageBasedSession Connector;

        public string MovePosSetCommand    { get { return "A:1{0}P{1}"; }   set {;} }
        public string MoveSettedPosCommand { get { return "G:"; }           set {;} }
        public string OriginCommand        { get { return "H:1"; }          set {;} } 
        public string StatusCommand        { get { return "Q:"; }           set {;} }
        public string ForceStopCommand     { get { return "L:E"; }          set {;} }

        #region Order
        public void Origin()
        {
            Write(OriginCommand);
        }

        public void MoveAbsPos(int pos)
        {
            Write(String.Format(MovePosSetCommand, pos > 0 ? "+" : "-", (int)Math.Abs(pos)));
            Write(MoveSettedPosCommand);
        }

        public void Wait2Arrive(int targetPos)
        {
            string targetstr = targetPos.ToString();
            string checkstr = CheckPosition();
            while (checkstr != targetstr)
            {
                Console.WriteLine("Current Pos : " + checkstr + "// Target Pos  " + targetstr);
                Thread.Sleep(100);
                checkstr = CheckPosition();
            }
        }

        public void Wait2ArriveEpsilon(int targetPos,double epsilon)
        {
            string targetstr = targetPos.ToString();
            string checkstr = CheckPosition();
            while ( Math.Abs( double.Parse(checkstr) - double.Parse(targetstr)) > epsilon )
            {
                Console.WriteLine("Current Pos : " + checkstr + "// Target Pos  " + targetstr);
                Thread.Sleep(50);
                checkstr = CheckPosition();
            }
        }

        public void SetXSpeed(int speed, int acc)
        {
            string input = String.Format("D:1S{0}F{1}R{2}", speed, speed, acc);
            Write(input);
        }

        public string Status()
        {
            return Query(StatusCommand);
        }

        public void ForceStop()
        {
            Connector.Write(ForceStopCommand);
        }
        #endregion

        #region Command

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

        #region Connect
        public bool XStageConnect(string port)
        {
            try
            {
                Connector = (MessageBasedSession)ResourceManager.GetLocalManager().Open("COM" + port);
                //Connector = (MessageBasedSession)ResourceManager.GetLocalManager().Open("ASRL5::INSTR");
                Connector.Timeout = 5000;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool XstageRelease()
        {
            if (Connector != null)
            {
                Connector.Dispose();
                return true;
            }
            return false;
        }
        #endregion

        #region SubMethod
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
        #endregion

    }
}
