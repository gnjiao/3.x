using System;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class EquHistoImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            HImage enhancedImage = image.EquHistoImage();

            return enhancedImage;
        }
    }
}