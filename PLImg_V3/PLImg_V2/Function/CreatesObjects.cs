using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALSA.SaperaLT.SapClassBasic;
using IDalsaCamera;

namespace PLImg_V2
{
    public class CreatesObjects : IDalsaCamera.IObjectMana
    {
        public void DestroysObjects(IMemberDefine dalsamem)
        {

            if (dalsamem.Xfer != null)
            {
                dalsamem.Xfer.Destroy();
                dalsamem.Xfer.Dispose();
            }

            if (dalsamem.AcqDevice != null)
            {
                dalsamem.AcqDevice.Destroy();
                dalsamem.AcqDevice.Dispose();
            }

            if (dalsamem.Acquisition != null)
            {
                dalsamem.Acquisition.Destroy();
                dalsamem.Acquisition.Dispose();
            }

            if (dalsamem.Buffers != null)
            {
                dalsamem.Buffers.Destroy();
                dalsamem.Buffers.Dispose();
            }

            if (dalsamem.View != null)
            {
                dalsamem.View.Destroy();
                dalsamem.View.Dispose();
            }

            if (dalsamem.ServerLocation != null)
            {
                dalsamem.ServerLocation.Dispose();
            }
        }
       
        public void CreatEndSqObject(SapBuffer buf, SapTransfer xfer, SapView view)
        {
            buf.Create();
            xfer.Create();
            //view.Create();
        }

        public void CreateLocation(SapLocation loc, string serverName, int index)
        {
            if (serverName == null || index < 0)
            { }
            loc = new SapLocation(serverName, index);
        }

        public void CreateAcqDevice(SapAcqDevice camera, SapLocation location, string configFileName)
        {
            if (location == null || configFileName == null)
            { }
            camera = new SapAcqDevice(location, configFileName);
        }

        public void CreateAcqusition(SapAcquisition acq, SapLocation location, string configFileName)
        {
            if (location == null || configFileName == null)
            { }
            acq = new SapAcquisition(location, configFileName);
        }

        public void CreateBuffer(SapBuffer buf, int count, SapXferNode srcNode, SapBuffer.MemoryType type)
        {
            if (count <=0 || srcNode == null)
            { }
            buf = new SapBuffer(1, srcNode, SapBuffer.MemoryType.ScatterGather);
        }

        public void CreateTranfer(SapTransfer xfer, SapAcqDevice acqDevice, SapBuffer buf)
        {
            if (acqDevice == null || buf == null)
            { }
            xfer = new SapAcqDeviceToBuf(acqDevice, buf);
        }

        public void CreateView(SapView view, SapBuffer buffer)
        {
            if (buffer == null)
            { }
            view = new SapView(buffer);
        }
    }
}
