using System;
using System.Collections.Generic;
using System.Linq;
using HalconDotNet;
using Core.Collections.Generic;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SelectHorizontalLinePairBoundaryRegionProcessor: IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            var untionRect1 = region.Union1().GetSmallestRectangle1Region();
            

            var lines = region.SelectShape("width", "and", LineWidthMin, LineWidthMax);
            var lines2 = lines.SelectShape("height", "and", LineHeightMin, LineHeightMax);
            var sortedLines = lines2.SortRegion("character", "true", "row");
            var lineList = sortedLines.ToList();

            var linePairs = GetLinePairs(lineList);

            var matchedLinePairs = GetMatchedLinePairs(linePairs);

//            var pair = matchedLinePairs[PairIndex];

            var boundaryRegions = new List<HRegion>();
            foreach (var matchedLinePair in matchedLinePairs)
            {
                var row1 = matchedLinePair.Item1.GetRow1();
                var row2 = matchedLinePair.Item2.GetRow2();
                var maxDistance = row2 - row1;

                var boundary = matchedLinePair.Item1.Union2(matchedLinePair.Item2).ClosingRectangle1(1, maxDistance);
                boundaryRegions.Add(boundary);
            }

            if (!IsComplement)
            {
                return boundaryRegions[PairIndex];
            }

            var avgArea = boundaryRegions.Average(x => x.Area);

            var boundaryUnion = boundaryRegions.Union();
            var differUnion = untionRect1.Difference(boundaryUnion);
            var differBoundaryRegions = differUnion.Connection();
            var differBoundaryRegions2 = differBoundaryRegions.SelectShape("area", "and", avgArea, 9999999999);
            var differBoundaryRegions3 = differBoundaryRegions2.SortRegion("character", "true", "row");
            var differBoundaryRegionsList = differBoundaryRegions3.ToList();
            return differBoundaryRegionsList[PairIndex];


            //            var row1 = pair.Item1.GetRow1();
            //            var row2 = pair.Item2.GetRow2();
            //            var maxDistance = row2 - row1;
            //
            //            var boundary = pair.Item1.Union2(pair.Item2).ClosingRectangle1(1, maxDistance);
            //            return boundary;
        }

        private List<Tuple<HRegion, HRegion>> GetMatchedLinePairs(List<Tuple<HRegion, HRegion>> linePairs)
        {
            List<Tuple<HRegion, HRegion>> matchedLinePairs = new List<Tuple<HRegion, HRegion>>();

            foreach (var linePair in linePairs)
            {
                var line1Center = linePair.Item1.GetCenterPoint();
                var line2Center = linePair.Item2.GetCenterPoint();

                var distance = (line1Center - line2Center).Length;
                if (distance > LinePairDistanceMin && distance < LinePairDistanceMax)
                {
                    matchedLinePairs.Add(linePair);
                }
            }
            return matchedLinePairs;
        }

        private static List<Tuple<HRegion, HRegion>> GetLinePairs(IList<HRegion> lineList)
        {
            List<Tuple<HRegion, HRegion>> linePairs = new List<Tuple<HRegion, HRegion>>();

            HRegion lastLine = null;
            foreach (var line in lineList)
            {
                if (lastLine == null)
                {
                    lastLine = line;
                    continue;
                }

                linePairs.Add(new Tuple<HRegion, HRegion>(lastLine, line));
                lastLine = line;
            }
            return linePairs;
        }

        public double LineWidthMin { get; set; }
        public double LineWidthMax { get; set; }

        public double LineHeightMin { get; set; }
        public double LineHeightMax { get; set; }

        public double LinePairDistanceMin { get; set; }
        public double LinePairDistanceMax { get; set; }

        public int PairIndex { get; set; }

        public bool IsComplement { get; set; }
    }
}