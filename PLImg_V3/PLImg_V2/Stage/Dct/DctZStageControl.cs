using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLImg_V2
{
    public class DctZStageControl : IDctXYZStage.IZStageOrder
    {
        public void ZForceStop()
        {
            
        }

        public void ZMoveAbsPos(int pos)
        {
            
        }

        public void ZOrigin()
        {
            
        }

        public void ZSetSpeed(int speed, int acc)
        {
            
        }

        public bool ZStageConnect(string port)
        {
            return true;
        }

        public bool ZStageRelease()
        {
            return true;
        }

        public string ZStatus()
        {
            return "OK";
        }

        public void ZWait2Arrive(int targetPos)
        {
            
        }

        public void ZWait2ArriveEpsilon(int targetPos, double epsilon)
        {
            
        }
    }
}
