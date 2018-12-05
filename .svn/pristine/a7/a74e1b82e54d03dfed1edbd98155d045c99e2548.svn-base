using System;
using System.Windows.Markup;
using HalconDotNet;
using Core;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class SingleSvmOcrDataCodeExtractor: IDataCodeExtractor
    {
        public string FindDataCode(HImage image)
        {
            //
            if (ClassifierFileName.IsNullOrEmpty())
                return "";

            //
            var domain = image.GetDomain();
            if (domain.Area < 0.000001)
                return "";

            //
            if (RegionExtractor==null)
                return "";

            var croppedImage = image.CropDomain();

            if (ImageFilter != null)
            {
                var filteredImage = ImageFilter.Process(croppedImage);
                croppedImage.Dispose();
                croppedImage = filteredImage;
            }

            var region = RegionExtractor.Extract(croppedImage);

            var ocr = new HOCRSvm(ClassifierFileName);
            ocr.ReadOcrClassSvm(ClassifierFileName);

            var result = ocr.DoOcrSingleClassSvm(region, croppedImage, 1);

            croppedImage.Dispose();
            region.Dispose();

            if (result.Length == 0)
                return "";

            string code = result.SArr[0];

            return code;
        }

        [Obsolete]
        private void UseHOperationSet(HImage croppedImage)
        {
#pragma warning disable 219
            HTuple hv_OCRHandle = null, hv_Width = null;

            HTuple hv_Height = null, hv_Class = null;
#pragma warning restore 219
            HOperatorSet.ReadOcrClassSvm(ClassifierFileName, out hv_OCRHandle);

            HOperatorSet.DoOcrSingleClassSvm(croppedImage, croppedImage, hv_OCRHandle, 1,
                out hv_Class);
            //Clear the classifier from memory
            HOperatorSet.ClearOcrClassSvm(hv_OCRHandle);
        }

        public string ClassifierFileName { get; set; }

        public IImageFilter ImageFilter { get; set; }

        public IRegionExtractor RegionExtractor { get; set; }
    }
}