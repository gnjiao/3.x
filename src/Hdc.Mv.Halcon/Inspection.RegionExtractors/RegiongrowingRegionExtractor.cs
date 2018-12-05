using System;
using HalconDotNet;
using System.ComponentModel;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class RegiongrowingRegionExtractor : RegionExtractorBase, IRegionExtractor
    {
        protected override HRegion ExtractInner(HImage image)
        {
            var region = image.Regiongrowing(Row, Column, Tolerance, MinSize);
            return region;
        }
        [Description("����������֮��Ĵ�ֱ����")]
        public int Row { get; set; } = 3;

        [Description("����������֮���ˮƽ����")]
        public int Column { get; set; } = 3;

        [Description("���ڵ�Ҷ�ֵ���ݲ����ֵ��1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 12.0, 14.0, 18.0, 25.0")]
        public double Tolerance { get; set; } = 6.0;

        [Description("����������С�ߴ磬����ֵ�� 1, 5, 10, 20, 50, 100, 200, 500, 1000")]
        public int MinSize { get; set; } = 100;
    }
}