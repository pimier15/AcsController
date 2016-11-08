using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLImg_V2
{
    class DumyStageControl : ISigmaKokiStage.ISigmakokiStageUnit
    {
        public string MovePosSetCommand { get { return "A:1{0}P{1}"; } set {; } }
        public string MoveSettedPosCommand { get { return "G:"; } set {; } }
        public string OriginCommand { get { return "H:1"; } set {; } }
        public string StatusCommand { get { return "Q:"; } set {; } }
        public string ForceStopCommand { get { return "L:E"; } set {; } }


        public void ForceStop()
        {
        }

        public void MoveAbsPos(int pos)
        {
            Thread.Sleep(pos);
        }

        public void Origin()
        {
        }

        public string Query(string command)
        {
            return null;
        }

        public string Read()
        {
            return null;
        }

        public void SetXSpeed(int speed, int acc)
        {
        }

        public string Status()
        {
            return null;
        }

        public void Wait2Arrive(int targetPos)
        {
            Thread.Sleep(targetPos);
        }

        public void Wait2ArriveEpsilon(int targetPos, double epsilon)
        {
            Thread.Sleep(targetPos);
        }
        public void Write(string command)
        {
        }

        public bool XStageConnect(string port)
        {
            return true;
        }

        public bool XstageRelease()
        {
            return true;
        }

        public bool RStageConnect( string port )
        {
            return true;
        }

        public bool RstageRelease( )
        {
            return true;
        }

        public void SetRSpeed( int speed, int acc )
        {
        }
    }
}
