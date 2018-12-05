using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Hdc.Mv.Halcon.DefectDetection
{
    public class TrainSvmSample
    {
        public static void TrainingSvmSample()
        {
            HClassSvm SVMHandleReduced1 = new HClassSvm(5, "rbf", 0.01, 0.0005, 1, "novelty-detection", "normalization", 5);
            //SVMHandleReduced1.CreateClassSvm(5, "rbf", 0.01, 0.0005, 1, "novelty-detection", "normalization", 5);
            for (int i = 1; i < 7; i++)
            {
                HImage img = new HImage();
                img.ReadImage(@"D:\HDCProgramFiles\ABU_\ABU_ Visual inspectio\B167Pic\R_Right\PositiveRCorner\" + i.ToString() + ".tif");
                img = img.ZoomImageFactor(0.5, 0.5, "constant");
                HTuple w1, h1;
                img.GetImageSize(out w1, out h1);
                HRegion rectRegion = new HRegion();
                rectRegion.GenRectangle1(0, 0, h1, w1);
                HImage imgCharacter = ExtractedCharacter.Extracted(img);
                SVMHandleReduced1.AddSamplesImageClassSvm(imgCharacter, rectRegion);
                rectRegion.Dispose();
                img.Dispose();
                imgCharacter.Dispose();
            }
            SVMHandleReduced1.TrainClassSvm(0.001, "default");
            SVMHandleReduced1.ReduceClassSvm("bottom_up", 2, 0.001);
            SVMHandleReduced1.WriteClassSvm("B://SVMHandleReducedfromCNew#ForR_right.gsc");
        }
    }
}
