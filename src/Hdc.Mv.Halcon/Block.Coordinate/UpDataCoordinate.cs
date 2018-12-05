using System;
using System.Windows;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;

namespace Hdc.Mv.Halcon
{
    public class UpDataCoordinate
    {
        private void CalculateTheActualRectanglePoint(Line actural, CoorinateResult coorinateResult, out Point actualP1,
            out Point actualP2)
        {
            var p1 = new Point
            {
                X = actural.X1 - coorinateResult.Column,
                Y = actural.Y1 - coorinateResult.Row
            };

            var p2 = new Point
            {
                X = actural.X2 - coorinateResult.Column,
                Y = actural.Y2 - coorinateResult.Row
            };

            actualP1 = coorinateResult.Coordinate.GetOriginalPoint(p1); //

            actualP2 = coorinateResult.Coordinate.GetOriginalPoint(p2); //
        }

        private void CalculateTheActualCirclePoint(Point p1, CoorinateResult coorinateResult, out Point outpoint)
        {
            outpoint = new Point();
            outpoint = coorinateResult.Coordinate.GetOriginalPoint(p1); //
        }

        public RegionOfInterest ChangeRoi2Newpic(RegionOfInterest oldRegionOfInterest, CoorinateResult coorinateResult)
        {
            Point actualRectangleP1, actualRectangleP2;

            var egionOfInterestReturn = new RegionOfInterest {RoiType = oldRegionOfInterest.RoiType};

            switch (oldRegionOfInterest.RoiType)
            {
                case RegionOfInterestType.rectangle1:
                    CalculateTheActualRectanglePoint(oldRegionOfInterest.ActuaLine, coorinateResult,
                        out actualRectangleP1, out actualRectangleP2);

                    egionOfInterestReturn.Row2 = actualRectangleP2.Y;
                    egionOfInterestReturn.Row1 = actualRectangleP1.Y;
                    egionOfInterestReturn.Column2 = actualRectangleP2.X;
                    egionOfInterestReturn.Column1 = actualRectangleP1.X;

                    break;

                case RegionOfInterestType.rectangle2:
                    CalculateTheActualRectanglePoint(oldRegionOfInterest.ActuaLine, coorinateResult,
                        out actualRectangleP1, out actualRectangleP2);

                    egionOfInterestReturn.Row = actualRectangleP2.Y - oldRegionOfInterest.Length1;
                    egionOfInterestReturn.Column = actualRectangleP2.X - oldRegionOfInterest.Length2;
                    egionOfInterestReturn.Phi =
                        coorinateResult.Coordinate.GetCoordinateAngle() + oldRegionOfInterest.Phi;

                    break;

                case RegionOfInterestType.circle:
                    var pt = new Point()
                    {
                        X = oldRegionOfInterest.Column,
                        Y = oldRegionOfInterest.Row
                    };
                    CalculateTheActualCirclePoint(pt, coorinateResult, out var actualCircleP);
                    egionOfInterestReturn.Row = actualCircleP.Y;
                    egionOfInterestReturn.Column = actualCircleP.X;
                    break;

                case RegionOfInterestType.ellipse:


                    break;
                case RegionOfInterestType.line:

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return egionOfInterestReturn;
        }
    }
}
