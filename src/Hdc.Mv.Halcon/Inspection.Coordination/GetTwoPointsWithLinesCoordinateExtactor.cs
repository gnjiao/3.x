using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;
using Core.Linq;
using Hdc.Mv.Halcon;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("EdgeSearchingDefinition")]
    public class GetTwoPointsWithLinesCoordinateExtactor : ICoordinateExtactor
    {
        public string Name { get; set; }
        public string CoordinateName { get; set; }

        public IRelativeCoordinate Extact(HImage image, IRelativeCoordinate coordinate,
            double pixelCellSideLengthInMillimeter)
        {
            var inspector3 = InspectorFactory.CreateEdgeInspector("Hal");

           // RegionSearchingDefinition.UpdateRelativeCoordinate(coordinate);
            foreach (var EdgeSearchingDefinition in Definitions)
            {
                EdgeSearchingDefinition.UpdateRelativeCoordinate(coordinate);
            }
            var CoordinateLines = inspector3.SearchEdges(image, Definitions);
            
            Point originPoint = new Point();
            switch (PointNumber)
            {
                case 1:
                    foreach (var edge in CoordinateLines)
                    {
                        originPoint.X = edge.EdgeLine.X1;
                        originPoint.Y = edge.EdgeLine.Y1;
                    }
                    break;
                case 2:
                    originPoint=GetCoordPointWith2Points(CoordinateLines);
                    break;
                case 3:
                    throw new NotImplementedException();
/*
                    break;
*/
                case 4:
                    throw new NotImplementedException();
                    //                                    
/*
                    break;
*/
                case 5:
                    throw new NotImplementedException();
                    //                                    
/*
                    break;
*/
            }
            //_coordinate  就是返回的二次坐标  CoordAngle是二次坐标的角度，
            //如果想用初定位坐标的角度的就是coordinate.GetCoordinateAngle(),其中coordinate就是初定位的坐标
            //使用初定位坐标
            var _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
                    originPoint.X, originPoint.Y, coordinate.GetCoordinateAngle());
            //var _coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
            //        originPoint.X, originPoint.Y, CoordAngle);
            if (_coordinate == null) throw new ArgumentNullException(nameof(_coordinate));
            return _coordinate;
        }
        public Collection<EdgeSearchingDefinition> Definitions { get; set; } = new Collection<EdgeSearchingDefinition>();
        public int  PointNumber { get; set; } = 2;
        private double CoordAngle { get; set; }
        private Point GetCoordPointWith2Points(IList<EdgeSearchingResult> CoordinateLines)
        {
            Point originPoint = new Point();
            int num = CoordinateLines.Count / 2;
            List<Line> lpTB = new List<Line>();
            List<Line> lpLR = new List<Line>();
            List<Line> rpTB = new List<Line>();
            List<Line> rpLR = new List<Line>();
            foreach (var edgeline in CoordinateLines)
            {
                string lineName = edgeline.Definition.Name;
                if (lineName.Contains("LPTB"))
                {
                    Line _line = new Line();
                    _line = edgeline.EdgeLine.X1 < edgeline.EdgeLine.X2
                                 ? edgeline.EdgeLine
                                 : edgeline.EdgeLine.Reverse();
                    lpTB.Add(_line);
                }
                if (lineName.Contains("LPLR"))
                {
                    Line _line = new Line();
                    _line = edgeline.EdgeLine.Y1 < edgeline.EdgeLine.Y2
                                 ? edgeline.EdgeLine
                                 : edgeline.EdgeLine.Reverse();
                    lpLR.Add(_line);
                }
                if (lineName.Contains("RPTB"))
                {
                    Line _line = new Line();
                    _line = edgeline.EdgeLine.X1 < edgeline.EdgeLine.X2
                                 ? edgeline.EdgeLine
                                 : edgeline.EdgeLine.Reverse();
                    rpTB.Add(_line);
                }
                if (lineName.Contains("RPLR"))
                {
                    Line _line = new Line();
                    _line = edgeline.EdgeLine.Y1 < edgeline.EdgeLine.Y2
                                 ? edgeline.EdgeLine
                                 : edgeline.EdgeLine.Reverse();
                    rpLR.Add(_line);
                }
            }
            Line lphorizontalMiddleLines = new Line();
            Line lpverticalMiddleLines = new Line();
            Line rphorizontalMiddleLines = new Line();
            Line rpverticalMiddleLines = new Line();


            lphorizontalMiddleLines = lpTB[0].GetMiddleLineUsingAngle(lpTB[1]);
            lpverticalMiddleLines = lpLR[0].GetMiddleLineUsingAngle(lpLR[1]);
            rphorizontalMiddleLines = rpTB[0].GetMiddleLineUsingAngle(rpTB[1]); ;
            rpverticalMiddleLines = rpLR[0].GetMiddleLineUsingAngle(rpLR[1]); ;

            Point leftPoint = new Point();
            Point rightPoint = new Point();
            leftPoint = lphorizontalMiddleLines.IntersectionWith(lpverticalMiddleLines);
            rightPoint = rphorizontalMiddleLines.IntersectionWith(rpverticalMiddleLines);
            originPoint.X = (leftPoint.X + rightPoint.X) / 2;
            originPoint.Y = (leftPoint.Y + rightPoint.Y) / 2;
            Vector lineangle = new Vector(rightPoint.X- leftPoint.X , rightPoint.Y- leftPoint.Y);
            CoordAngle = lineangle.GetAngleToX();                                                       //改
            return originPoint;
        }
    }
}
