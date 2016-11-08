using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALSA.SaperaLT.SapClassBasic;

namespace IDalsaCamera
{
    public interface IObjectSetting
    {
        void AcqusitionSetting(SapAcquisition acquisition);

        void XferSetting(SapAcqToBuf xfer, SapAcquisition acquisition, SapBuffer buffers);

    }
}
