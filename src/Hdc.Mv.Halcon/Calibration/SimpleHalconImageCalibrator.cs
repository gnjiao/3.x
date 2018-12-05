using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv
{
    public class SimpleHalconImageCalibrator : IHalconImageCalibrator
    {
        public HImage Calibrate(HImage image)
        {
            HImage finalImage = image.Clone();

//            if (MirrorRow)
//            {
//                var mirrorX = finalImage.MirrorImage("row");
//                finalImage.Dispose();
//                finalImage = mirrorX;
//            }
//
//            if (MirrorColumn)
//            {
//                var mirrorY = finalImage.MirrorImage("column");
//                finalImage.Dispose();
//                finalImage = mirrorY;
//            }
//
//            if (CropX != 0 || CropY != 0 || CropWidth != 0 || CropHeight != 0)
//            {
//                int width = CropWidth;
//                int height = CropHeight;
//
//                if (CropWidth == 0)
//                    width = image.GetWidth();
//
//                if (height == 0)
//                    height = image.GetHeight();
//
//                var croppedImage = finalImage.CropPart(CropY, CropX, width, height);
//                finalImage.Dispose();
//                finalImage = croppedImage;
//            }

            finalImage = finalImage.MirrorAndCrop(MirrorRow, MirrorColumn, CropX, CropY, CropWidth, CropHeight);

            return finalImage;
        }

        public int CropX { get; set; }
        public int CropY { get; set; }
        public int CropWidth { get; set; }
        public int CropHeight { get; set; }
        public bool MirrorRow { get; set; }
        public bool MirrorColumn { get; set; }
    }
}