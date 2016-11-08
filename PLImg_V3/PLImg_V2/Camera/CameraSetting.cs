using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;

namespace PLImg_V2
{
    public class CameraSetting : IDalsaCamera.ICameraSetting
    {
        public void SetExposureTime(MessageBasedSession mbSession, double time)
        {
            mbSession.Query("set " + time.ToString() + "\r\n");
        }

        public void SetLineRate(MessageBasedSession mbSession, int rate)
        {
            mbSession.Query("ssf " + rate.ToString() + "\r\n");
        }
    }
}
