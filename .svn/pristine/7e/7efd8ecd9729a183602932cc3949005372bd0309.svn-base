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
    public class SVM_Classifier : IRegionExtractor
    {
        public string Location { get; set; } = "Corner_R";
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
            var ersionRegion = selectedRegions.ErosionRectangle1(230, 32);
            int row1, column1, row2, column2;
            ersionRegion.SmallestRectangle1(out row1, out column1, out row2, out column2);
            HRegion rectRegion = new HRegion();
            rectRegion.GenRectangle1((double)row1, column1, row2, column2);
            HImage imgReduce = image.ReduceDomain(rectRegion);
            HImage imgZoom = imgReduce.ZoomImageFactor(0.5, 0.5, "constant");
            HImage imagecharacter = ExtractedCharacter.Extracted(imgZoom);
            HRegion errors = imagecharacter.ClassifyImageClassSvm(SVMHandleReduced);
            errors = errors.ZoomRegion(2, 2);
            regionBinary.Dispose();
            regionConnection.Dispose();
            rectRegion.Dispose();
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
