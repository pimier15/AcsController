using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;

namespace PLImg_V2
{
    class ConnectVISA : IDalsaCamera.IConnectVISA
    {
        public void Connect2VISA(ref MessageBasedSession mbsession, string path)
        {
            mbsession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(path);
        }
    }
}
