using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALSA.SaperaLT.SapClassBasic;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading.Tasks;

namespace PLImg_V2
{
    public class GrabMana : IDalsaCamera.IGrabMana
    {
        string DirPath;
        public GrabMana(string dirpath)
        {
            DirPath = dirpath;
        }

        public void SetDirPath(string dirpath)
        {
            DirPath = dirpath;
        }

        public byte[] DataTransFromBuffer(SapBuffer buff)
        {
            byte[] output = new byte[buff.Width*buff.Height];
            GCHandle outputAddr = GCHandle.Alloc( output, GCHandleType.Pinned); // output 의 주소 만듬
            IntPtr pointer = outputAddr.AddrOfPinnedObject(); // 
            buff.ReadRect(0,0,buff.Width,buff.Height, pointer);
            
            Marshal.Copy(pointer, output, 0, output.Length);
            outputAddr.Free();
            return output;
        }

        public byte[] DataTransFromBufferOneLine(SapBuffer buff)
        {
            byte[] output = new byte[buff.Width];
            GCHandle outputAddr = GCHandle.Alloc(output, GCHandleType.Pinned); // output 의 GC주소 만듬
            IntPtr pointer = outputAddr.AddrOfPinnedObject();
            //buff.ReadRect(0, 0, buff.Width, 2, pointer);     // 이걸로 안하면 무조건 중간에 멈춘다.
            int readnum = 0;
            buff.ReadLine( 0, 0, 12287, 0, pointer, out readnum );
            Marshal.Copy(pointer, output, 0, output.Length); // Pointer에서 가리키는 첫번쨰 메모리 주소에서부터 Length 만큼 카피를 한다.
            outputAddr.Free();
            return output;

        }

        public async void TempSaveGrabDataLineMode(byte[] inputArr, int buffNum, int scanNum)
        {
            await Task.Run(() => {
                string path = DirPath + scanNum.ToString("D3") + "_" + buffNum.ToString("D3") + ".dat";
                Stream outStream = new FileStream(path, FileMode.Create);
                outStream.Write(inputArr, 0, inputArr.Length);
                outStream.Dispose();
            });
            
        }

        public async void TempSaveGrabDataFullMode(byte[] inputArr, int lineNum,int unitNum, int buffNum)
        {
            await Task.Run(() => {
                string path = DirPath +"\\"+ lineNum.ToString("D2") + "_" + unitNum.ToString("D2") +"_" + buffNum.ToString("D2")+ ".dat";
                Stream outStream = new FileStream(path, FileMode.Create);
                outStream.Write(inputArr, 0, inputArr.Length);
                outStream.Dispose();
            });
        }
    }
}
