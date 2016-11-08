using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using IDalsaCamera;

namespace PLImg_V2
{
    public class Display : IDalsaCamera.IDisplayEmgu
    {
        void IDisplayEmgu.Display(ImageBox imgBox, Image<Gray, byte> inputImg)
        {
            imgBox.Image = inputImg;
        }

        void IDisplayEmgu.Display(ImageBox imgBox, byte[] inputArr, int Width, int Height)
        {
            Image<Gray, byte> inputImg = new Image<Gray, byte>(Width, Height);
            inputImg.Bytes = inputArr;
            imgBox.Image   = inputImg; 
        }
    }
}
