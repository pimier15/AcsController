using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDctXYZStage
{
    public interface IZStageOrder
    {
        #region Connect
        bool ZStageConnect(string port);
        bool ZStageRelease();

        #endregion

        void ZMoveAbsPos(int pos);
        void ZWait2Arrive(int targetPos);
        void ZWait2ArriveEpsilon(int targetPos, double epsilon);
        void ZOrigin();
        void ZSetSpeed(int speed, int acc);
        void ZForceStop();
        string ZStatus();
    }
}
