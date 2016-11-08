using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALSA.SaperaLT.SapClassBasic;

namespace IDalsaCamera
{
    public interface IGrabMana
    {
        byte[] DataTransFromBuffer(SapBuffer buff);
        byte[] DataTransFromBufferOneLine(SapBuffer buff);
        void TempSaveGrabDataLineMode(byte[] inputArr, int buffNum, int scanNum);
        void TempSaveGrabDataFullMode(byte[] inputArr, int lineNum, int unitNum, int buffNum);
        void SetDirPath(string dirpath);
    }
}
