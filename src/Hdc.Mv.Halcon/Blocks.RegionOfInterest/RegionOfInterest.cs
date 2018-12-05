using System;
using HalconDotNet;

namespace Hdc.Mv.Halcon.Blocks.RegionOfInterest
{
    [Serializable]
    public class RegionOfInterest
    {
        private HDrawingObject DrawObject { get; set; }
        public RegionOfInterestType RoiType { get; set; }
        public double Row { get; set; }
        public double Column { get; set; }
        public double Row1 { get; set; }
        public double Column1 { get; set; }
        public double Row2 { get; set; }
        public double Column2 { get; set; }
        public double Radius { get; set; }
        public double Radius1 { get; set; }
        public double Radius2 { get; set; }
        public double Phi { get; set; }
        public double Length1 { get; set; }
        public double Length2 { get; set; }
        public bool Initialize { get; set; } = false;
        public Line ActuaLine { get; set; } 

        public RegionOfInterest()
        {

        }

        public RegionOfInterest(HDrawingObject hDrawObject)
        {
            DrawObject = hDrawObject;

            string roiTypeName = DrawObject.GetDrawingObjectParams("type");

            Enum.TryParse<RegionOfInterestType>(roiTypeName,true, out var roiType);

            RoiType = roiType;

            switch (RoiType)
            {
                case RegionOfInterestType.rectangle1:

                    Row1 = DrawObject.GetDrawingObjectParams("row1");
                    Column1 = DrawObject.GetDrawingObjectParams("column1");
                    Row2 = DrawObject.GetDrawingObjectParams("row2");
                    Column2 = DrawObject.GetDrawingObjectParams("column2");

                    ActuaLine = new Line(Column1, Row1, Column2, Row2);
                    break;

                case RegionOfInterestType.rectangle2:

                    Row = DrawObject.GetDrawingObjectParams("row");
                    Column = DrawObject.GetDrawingObjectParams("column");
                    Phi = DrawObject.GetDrawingObjectParams("phi");
                    Length1 = DrawObject.GetDrawingObjectParams("length1");
                    Length2 = DrawObject.GetDrawingObjectParams("length2");

                    ActuaLine = new Line(Column - Length2, Row - Length1, Column + Length2, Row + Length1);                    

                    break;

                case RegionOfInterestType.circle:

                    Row = DrawObject.GetDrawingObjectParams("row");
                    Column = DrawObject.GetDrawingObjectParams("column");
                    Radius = DrawObject.GetDrawingObjectParams("radius");
                    break;

                case RegionOfInterestType.ellipse:

                    Row = DrawObject.GetDrawingObjectParams("row");
                    Column = DrawObject.GetDrawingObjectParams("column");
                    Phi = DrawObject.GetDrawingObjectParams("phi");
                    Radius1 = DrawObject.GetDrawingObjectParams("radius1");
                    Radius2 = DrawObject.GetDrawingObjectParams("radius2");

                    break;
                case RegionOfInterestType.line:

                    Row1 = DrawObject.GetDrawingObjectParams("row1");
                    Column1 = DrawObject.GetDrawingObjectParams("column1");
                    Row2 = DrawObject.GetDrawingObjectParams("row2");
                    Column2 = DrawObject.GetDrawingObjectParams("column2");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Initialize = true;
        }        

        public HDrawingObject CreateRoiDrawingObject()
        {
            if (!Initialize)
                return null;

            if (DrawObject != null)
            {
                DrawObject.Dispose();
                DrawObject = null;
            }

            switch (RoiType)
            {
                case RegionOfInterestType.rectangle1:

                    DrawObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.RECTANGLE1, Row1, Column1, Row2, Column2);
                    break;

                case RegionOfInterestType.rectangle2:

                    DrawObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.RECTANGLE2, Row, Column, Phi, Length1, Length2);
                    break;

                case RegionOfInterestType.circle:

                    DrawObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.CIRCLE, Row, Column, Radius);
                    break;

                case RegionOfInterestType.ellipse:

                    DrawObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.ELLIPSE, Row, Column, Phi, Radius1, Radius2);
                    break;
                case RegionOfInterestType.line:

                    DrawObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.LINE, Row1, Column1, Row2, Column2);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            DrawObject.SetDrawingObjectParams("color", "green");

            return DrawObject;
        }

        public HRegion RoiRegion {
            get
            {
                if (Initialize)
                {
                    if (DrawObject != null && DrawObject != IntPtr.Zero)
                        return new HRegion(DrawObject?.GetDrawingObjectIconic()); 
                    else
                        return new HRegion(CreateRoiDrawingObject()?.GetDrawingObjectIconic());
                }
                else
                {
                    return null;
                }

            }
        }
}
}
