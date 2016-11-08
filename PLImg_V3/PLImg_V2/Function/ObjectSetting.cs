using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALSA.SaperaLT.SapClassBasic;

namespace PLImg_V2
{
    public class ObjectSetting : IDalsaCamera.IObjectSetting
    {
        public void AcqusitionSetting(SapAcquisition acquisition)
        {
            acquisition.Create();
        }

        public void XferSetting(SapAcqToBuf xfer, SapAcquisition acquisition,SapBuffer buffers)
        {
            xfer = new SapAcqToBuf(acquisition, buffers);
            xfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;
            
        }
    }
}
