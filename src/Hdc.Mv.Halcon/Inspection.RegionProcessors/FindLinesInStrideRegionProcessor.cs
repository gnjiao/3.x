using System;
using System.Collections.Generic;
using System.Linq;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class FindLinesInStrideRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var lines = region.ToList();
            var targetLines = lines.Where(x =>
            {
                var rect2 = x.GetSmallestHRectangle2();
                return rect2.Length >= TargetLineLengthMin &&
                       rect2.Length <= TargetLineLengthMax &&
                       rect2.Width >= TargetLineWidthMin &&
                       rect2.Width <= TargetLineWidthMax;
            }).ToList();


            var foundLines = new List<HRegion>();
            foreach (var baseLine in targetLines)
            {
                var baseLineRect2 = baseLine.GetSmallestHRectangle2();
                if (baseLineRect2.Length >= BaseLineLengthMin &&
                    baseLineRect2.Length <= BaseLineLengthMax &&
                    baseLineRect2.Width >= BaseLineWidthMin &&
                    baseLineRect2.Width <= BaseLineWidthMax)
                {
                    var angleNearLines = new List<HRegion>();

                    foreach (var targetLine in targetLines)
                    {
                        var targetRect2 = baseLine.GetSmallestHRectangle2();

                        if (targetRect2.Angle >= baseLineRect2.Angle + TargetLineAngleOffsetMin &&
                            targetRect2.Angle <= baseLineRect2.Angle + TargetLineAngleOffsetMax)
                        {
                            angleNearLines.Add(targetLine);
                        }
                    }

                    if (!angleNearLines.Any())
                        continue;

                    var stride = new HRegion();
                    stride.GenRectangle2(baseLineRect2.Row, baseLineRect2.Column, baseLineRect2.Phi, StrideLength / 2, StrideWidth / 2);

                    var unionAngleNearLines = angleNearLines.Union();
                    var intersection = stride.Intersection(unionAngleNearLines);
                    var final = baseLine.Union2(intersection);
                    foundLines.Add(final);
                }
            }

            var regionTuple = foundLines.Concatenate();
            return regionTuple;
        }

        public double BaseLineLengthMin { get; set; } = 0;
        public double BaseLineLengthMax { get; set; } = 9999999;
        public double BaseLineWidthMin { get; set; } = 0;
        public double BaseLineWidthMax { get; set; } = 9999999;

        public double TargetLineLengthMin { get; set; } = 0;
        public double TargetLineLengthMax { get; set; } = 9999999;
        public double TargetLineWidthMin { get; set; } = 0;
        public double TargetLineWidthMax { get; set; } = 9999999;

        /// <summary>
        /// -360 to 360
        /// </summary>
        public double TargetLineAngleOffsetMin { get; set; } = -5;

        /// <summary>
        /// -360 to 360
        /// </summary>
        public double TargetLineAngleOffsetMax { get; set; } = 5;

        /// <summary>
        /// 2 x rect2.len1
        /// </summary>
        public double StrideLength { get; set; }

        /// <summary>
        /// 2 x rect2.len2
        /// </summary>
        public double StrideWidth { get; set; }
    }
}