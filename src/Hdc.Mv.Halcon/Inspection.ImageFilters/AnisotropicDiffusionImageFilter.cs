using System;
using HalconDotNet;
using System.ComponentModel;//yx

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class AnisotropicDiffusionImageFilter: ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            string modeString = null;
            switch (Mode)
            {
                    case AnisotropicDiffusionMode.Weickert:
                    modeString = "weickert";
                    break;
                    case AnisotropicDiffusionMode.Parabolic:
                    modeString = "parabolic";
                    break;
                    case AnisotropicDiffusionMode.PeronaMalik:
                    modeString = "perona-malik";
                    break;
            }

            HImage enhancedImage = image.AnisotropicDiffusion(modeString, Contrast,Theta, Iterations);
            return enhancedImage;
        }

        [DefaultValue("weickert")]
        [Browsable(true)] //yx
        [Description("扩散系数充当边缘幅值的函数，可选模式有：‘parabolic’, ‘perona - malik’, ‘weickert’")]
        public AnisotropicDiffusionMode Mode { get; set; } = AnisotropicDiffusionMode.Weickert;//

        [DefaultValue(5)]
        [Browsable(true)] //yx
        [Description("对比度参数，建议值：2.0, 5.0, 10.0, 20.0, 50.0, 100.0")]
        public double Contrast { get; set; } = 5.0;

        [DefaultValue(1)]
        [Browsable(true)] //yx
        [Description("时间间隔，建议值： 0.5, 1.0, 3.0")]
        public double Theta { get; set; } = 1.0;

        [DefaultValue(10)]
        [Browsable(true)] //yx
        [Description("迭代次数，建议值：1, 3, 10, 100, 500")]
        public int Iterations { get; set; } = 10;
    }
}