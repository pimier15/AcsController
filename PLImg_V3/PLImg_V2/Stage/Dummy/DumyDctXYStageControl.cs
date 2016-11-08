using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLImg_V2
{
    public class DumyDctXYStageControl : IDctXYZStage.IXYStageOrder
    {
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
            return true;
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
            Thread.Sleep(2000);
        }

        public void XYWait2ArriveEpsilon(int targetPos, int targetPosY, double epsilon)
        {
            Thread.Sleep((int)(epsilon));
        }
    }
}
