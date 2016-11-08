using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLImg_V2
{
    public class FullScanData
    {
        public int PosXStart;
        public int PosYStart;
        public int PosXEnd;
        
        public readonly int BuffW = 12288;
        public readonly int BuffH = 1024;
        public readonly int YStep = (int)(12288.0 * 2.5); // Unit um
        public readonly int OneUnitBuffNum = 12;
        public readonly int OneLineBuffNum = 48;
        public readonly int BuffLimit = 11;
        public readonly int UnitLimit = 3;
        public int LineLimit = 3;
    }
}
