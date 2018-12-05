using System;
using HalconDotNet;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class LaplaceOfGaussImageFilter : IImageFilter
    {
        public HImage Process(HImage image)
        {
            HImage enhancedImage = image.LaplaceOfGauss(Sigma);

            return enhancedImage;
        }

        [DefaultValue(2)]
        [Browsable(true)] //yx
        [Description("高斯平滑参数, 建议值: 1.0, 1.5, 2.0, 2.5, 3.0, 4.0, 5.0")]
        public double Sigma { get; set; } = 2;
    }
}