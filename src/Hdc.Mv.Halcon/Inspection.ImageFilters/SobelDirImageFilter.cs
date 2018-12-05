using System;
using System.ComponentModel;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SobelDirImageFilter: ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            HImage dirImage;
            var dir = image.SobelDir(out dirImage, FilterType.ToString(), Size);
            dir.Dispose();
            return dirImage;
        }

        [DefaultValue(3)]
        [Browsable(true)]
        [Description(
            "滤波内核尺寸, " +
            "可选值有: 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39"
            )]
        public int Size { get; set; } = 3;

        [DefaultValue("sum_abs")]
        [Browsable(true)]
        [Description("滤波类型, " +
                     "'sum_abs', 'sum_abs_binomial', 'sum_sqrt', 'sum_sqrt_binomial', ")]
        public SobelDirFilterType FilterType { get; set; } = SobelDirFilterType.SumAbs;

    }
}