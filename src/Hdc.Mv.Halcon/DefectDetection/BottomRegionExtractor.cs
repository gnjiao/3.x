using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Shapes;
using HalconDotNet;
using Hdc.Mv.Halcon;
using Hdc.Mv.Halcon.DefectDetection;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class BottomRegionExtractor : IRegionExtractor
    {
        public string Location { get; set; } = "Bottom";
        public int LastPicNumber { get; set; } = 24;
        public string Svmaddress { get; set; }
        public string OkOrNg;


        public HRegion Extract(HImage image)
        {
            HClassSvm SVMHandleReduced = new HClassSvm();
            int usedThreshold;
            double DetecyionArea;
            switch (Location)
            {

                case "Corner_R":
                    SVMHandleReduced.ReadClassSvm(Svmaddress);
                    break;
                case "Top":
                    SVMHandleReduced.ReadClassSvm(Svmaddress);
                    break;
                case "Bottom":
                    SVMHandleReduced.ReadClassSvm(Svmaddress);
                    break;
                case "Side":
                    SVMHandleReduced.ReadClassSvm(Svmaddress);
                    break;


            }

            HRegion regionBinary = image.BinaryThreshold("max_separability", "light", out usedThreshold);
            var regionConnection = regionBinary.Connection();
            var selectedRegions = regionConnection.SelectShape("area", "and", 20000, 9999999);
            var fillupRegion = selectedRegions.FillUp();
            var erosionRegion1 = fillupRegion.ErosionCircle(9.5);
            var ersionRegion = fillupRegion.ErosionCircle(47.5);
            var differenceRegion = erosionRegion1.Difference(ersionRegion);
            HImage imgReduce = image.ReduceDomain(differenceRegion);
            HImage imgZoom = imgReduce.ZoomImageFactor(0.5, 0.5, "constant");
            HImage imagecharacter = ExtractedCharacter.Extracted(imgZoom);
            HRegion errors = imagecharacter.ClassifyImageClassSvm(SVMHandleReduced);
            errors = errors.ZoomRegion(2, 2);

            fillupRegion.Dispose();
            erosionRegion1.Dispose();
            ersionRegion.Dispose();
            regionBinary.Dispose();
            regionConnection.Dispose();
            imgReduce.Dispose();
            imgZoom.Dispose();
            imagecharacter.Dispose();
            if (errors.Area < 100)
            {
                return null;
            }
            else
            {
                HTuple row, col;
                //DetecyionArea = unionRegion.AreaCenter(out row, out col);
                DetecyionArea = errors.AreaCenter(out row, out col);
                //return unionRegion;
                return errors;
            }


        }
    }
}
