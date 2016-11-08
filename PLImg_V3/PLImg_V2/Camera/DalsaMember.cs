using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALSA.SaperaLT.SapClassBasic;

namespace PLImg_V2
{
    public class DalsaMember : IDalsaCamera.IMemberDefine
    {
        public  SapLocation     ServerLocation  { get;set; }
        public  SapAcqDevice    AcqDevice       { get;set; }
        public  SapAcquisition  Acquisition     { get;set; }
        public  SapBuffer       Buffers         { get;set; }
        public  SapAcqToBuf     Xfer            { get;set; }
        public  SapView         View            { get;set; }
    }
}
