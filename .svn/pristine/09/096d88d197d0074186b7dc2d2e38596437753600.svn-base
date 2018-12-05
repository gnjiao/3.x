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
    public class TopRegionExtractor : IRegionExtractor
    {
        public string Location { get; set; } = "Top";
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
            var regionFillup = selectedRegions.FillUp();
            var regionErosion = regionFillup.ErosionCircle(8.5);
            var regionErosion1 = regionFillup.ErosionCircle(51.5);
            var regionErosion2 = regionFillup.ErosionCircle(58.5);
            var imageReduced1 = image.ReduceDomain(regionErosion);
            var binaryRegion1=imageReduced1.BinaryThreshold("max_separability", "dark", out usedThreshold);
            var connectionRegion1 = binaryRegion1.Connection();
            var selectedRegions2=connectionRegion1.SelectShape("area", "and", 20000, 9999999);
            var dilationRegion = selectedRegions2.DilationCircle(8.5);
            var differenceRegion1 = regionErosion1.Difference(regionErosion2);
            var differenceRegion2 = regionErosion.Difference(differenceRegion1);
            var differenceRegion3 = differenceRegion2.Difference(dilationRegion);
            HImage imgReduce = image.ReduceDomain(differenceRegion3);
            HImage imgZoom = imgReduce.ZoomImageFactor(0.5, 0.5, "constant");
            HImage imagecharacter = ExtractedCharacter.Extracted(imgZoom);
            HRegion errors = imagecharacter.ClassifyImageClassSvm(SVMHandleReduced);
            errors = errors.ZoomRegion(2, 2);

            regionConnection.Dispose();
            selectedRegions.Dispose();
            regionFillup.Dispose();
            regionErosion.Dispose();
            regionErosion1.Dispose();
            regionErosion2.Dispose();
            binaryRegion1.Dispose();
            imageReduced1.Dispose();
            connectionRegion1.Dispose();
            dilationRegion.Dispose();
            selectedRegions2.Dispose();
            imageReduced1.Dispose();
            differenceRegion1.Dispose();
            differenceRegion3.Dispose();
            differenceRegion2.Dispose();
            differenceRegion1.Dispose();
            regionBinary.Dispose();
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
