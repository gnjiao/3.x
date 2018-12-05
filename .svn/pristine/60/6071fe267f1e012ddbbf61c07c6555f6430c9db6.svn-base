using System;
using HalconDotNet;
using Hdc.Mv.Halcon;
using System.ComponentModel;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SobelAmpImageFilter : ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            return image.SobelAmp(FilterType.ToHalconString(), Size);
        }

        [DefaultValue(3)]
        [Browsable(true)]
        [Description(
            "滤波内核的尺寸,可选值有: 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39"
            )]
        public int Size { get; set; } = 3;

        [DefaultValue("sum_abs")]
        [Browsable(true)]
        [Description("滤波类型,可选值有: 'sum_abs', 'sum_sqrt','x','y', 'sum_abs_binomial', 'sum_sqr_binomial','x_binomial' ,'y_binomial'")]
        public SobelAmpFilterType FilterType { get; set; }=SobelAmpFilterType.SumAbs;
    }
}