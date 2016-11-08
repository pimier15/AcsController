using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IDalsaCamera
{
    public interface ITransfer
    {
        byte[] AcqRawData();

        void SplitImg(int length, byte[] inputraw);

        void SaveTempSplitFile(string dirpath,List<byte> inputlist);

        void CollectSplitFile(string dirpath, int fileNum, string format);

        Bitmap IntegrateResource(List<byte> inputList);

    }
}
