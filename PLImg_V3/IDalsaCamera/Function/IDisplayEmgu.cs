using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;

namespace IDalsaCamera
{
    public interface IDisplayEmgu
    {
        void Display(ImageBox imgBox,Image<Gray, byte> inputImg);

        void Display(ImageBox imgBox, byte[] inputArr, int Width, int Height);
    }
}
