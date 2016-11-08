using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;

namespace IDalsaCamera
{
    public interface ICameraSetting
    {
        void SetExposureTime(MessageBasedSession mbSession, double time);
        void SetLineRate(MessageBasedSession mbSession, int rate);
    }
}
