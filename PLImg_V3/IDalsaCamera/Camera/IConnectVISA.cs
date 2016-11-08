using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;

namespace IDalsaCamera
{
    public interface IConnectVISA
    {
        void Connect2VISA(ref MessageBasedSession mbsession, string path);
    }
}
