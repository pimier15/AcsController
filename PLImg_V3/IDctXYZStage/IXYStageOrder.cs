using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDctXYZStage
{
    public interface IXYStageOrder
    {
        #region Connect
        bool XYStageConnect(string port);
        bool XYStageRelease();

        #endregion

        void XYMoveAbsPos(int posX,int posY);
        void XYWait2Arrive(int targetPosX, int targetPosY);
        void XYWait2ArriveEpsilon(int targetPos, int targetPosY,double epsilon);
        void XYOrigin();
        void XYSetSpeed(int speedX, int speedY, int accX , int accY); 
        void XYForceStop();
        string XYStatus();
    }
}
