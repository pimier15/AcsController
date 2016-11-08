using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALSA.SaperaLT.SapClassBasic;

namespace IDalsaCamera
{
    public interface IMemberDefine
    {
        SapLocation     ServerLocation   { get;set; }
        SapAcqDevice    AcqDevice        { get;set; }
        SapAcquisition  Acquisition      { get;set; }
        SapBuffer       Buffers          { get;set; }
        SapAcqToBuf     Xfer             { get;set; }
        SapView         View             { get;set; }
    }
}
