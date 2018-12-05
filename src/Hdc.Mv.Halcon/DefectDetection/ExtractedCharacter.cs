using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Hdc.Mv.Halcon.DefectDetection
{
     public class ExtractedCharacter
    {
        public static HImage Extracted(HImage Image)
        {


            HObject ImageMean=null;
            HObject ImageAbsDiff = null;
            HObject ImageMin = null;
            HObject ImageMean2 = null;
            HObject ImageEL = null;
            HObject ImageLE = null;
            HObject ImageES = null;
            HObject ImageSE = null;
            HObject ImageEE = null;
            HObject CharacterImage =null;
            HOperatorSet.MeanImage(Image,out ImageMean,9,9);
            HOperatorSet.AbsDiffImage(Image, ImageMean, out ImageAbsDiff, 3);
            HOperatorSet.GrayErosionShape(ImageAbsDiff, out ImageMin, 3, 3, "octagon");
            HOperatorSet.MeanImage(ImageMin, out ImageMean2, 9, 9);
            HOperatorSet.TextureLaws(Image,out ImageEL, "el", 5, 5);
            HOperatorSet.TextureLaws(Image, out ImageLE, "le", 5, 5);
            HOperatorSet.TextureLaws(Image, out ImageES, "es", 1, 5);
            HOperatorSet.TextureLaws(Image, out ImageSE, "se", 1, 7);
            HOperatorSet.TextureLaws(Image, out ImageEE, "ee", 2, 7);
            //HOperatorSet.Compose5(ImageMean2, ImageLE, ImageES, ImageSE, ImageEE, out CharacterImage);
            HOperatorSet.Compose5(ImageEL, ImageLE, ImageES, ImageSE, ImageEE, out CharacterImage);
            

            return new HImage(CharacterImage) ;
        }
    }
}
