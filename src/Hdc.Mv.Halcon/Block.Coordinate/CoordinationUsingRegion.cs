using System;
using System.ComponentModel;
using System.Windows;
using HalconDotNet;
using Hdc.Controls;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("CoordinationUsingRegion", BlockCatagory.Coordinate)]    
    public class CoordinationUsingRegionBlock: Block
    {       
        public override void Process()
        {
            if (Hregion == null)
            {
                Status = BlockStatus.Error;
                Message = "Region is null.";
                Exception = new BlockException("RegionExtractor is failure.");
                return;
            }
            
            var rect2 = Hregion.GetSmallestHRectangle2();
            var originPoint = new Point();
            var line = rect2.GetRoiLineFromRectangle2Phi();
            switch (RegionCenterDirection)
            {
                case Halcon.RegionCenterDirection.Center:
                    CoorinateResult=new CoorinateResult(rect2.Column,rect2.Row, Angle);
                    break;
                case Halcon.RegionCenterDirection.Left:
                    originPoint = line.X1 < line.X2 ? line.GetPoint1() : line.GetPoint2();
                    CoorinateResult = new CoorinateResult(originPoint.X, originPoint.Y, Angle);
                    break;
                case Halcon.RegionCenterDirection.Right:
                    originPoint = line.X1 > line.X2 ? line.GetPoint1() : line.GetPoint2();
                    CoorinateResult = new CoorinateResult(originPoint.X, originPoint.Y, Angle);                       
                    break   ;
                case Halcon.RegionCenterDirection.Up:
                    break;
                case Halcon.RegionCenterDirection.Down:
                    break;

            }

            CoorinateResult.Name = CoordinateName;

            CoorinateResult.Coordinate = RelativeCoordinateFactory.CreateCoordinateUsingPointAndAngle(
                originPoint.X, originPoint.Y, -rect2.Angle);
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {
        }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public HRegion Hregion { set; get; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public string CoordinateName { set; get; }


        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public RegionCenterDirection RegionCenterDirection { set; get; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public double Angle { set; get; } = 0;

        [OutputPort]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category(BlockPropertyCategories.Output)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CoorinateResult CoorinateResult { set; get; }
    }

    public enum RegionCenterDirection
    {
        Center,
        Left,
        Right,
        Up,
        Down
    }
}