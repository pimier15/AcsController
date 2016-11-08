using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALSA.SaperaLT.SapClassBasic;


namespace IDalsaCamera
{
    public interface IObjectMana
    {
        void DestroysObjects(IDalsaCamera.IMemberDefine dalsamem);
        void CreatEndSqObject(SapBuffer buf, SapTransfer xfer, SapView view);
        void CreateLocation(SapLocation loc, string serverName, int index);
        void CreateAcqusition(SapAcquisition acq, SapLocation location, string configFileName);
        void CreateAcqDevice(SapAcqDevice camera, SapLocation location, string configFileName);
        void CreateBuffer(SapBuffer buf, int count, SapXferNode srcNode, SapBuffer.MemoryType type);
        void CreateTranfer(SapTransfer xfer, SapAcqDevice acqDevice, SapBuffer buf);
        void CreateView(SapView view, SapBuffer buffer);
    }
}
