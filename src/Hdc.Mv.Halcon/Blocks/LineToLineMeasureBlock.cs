using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Hdc.Controls;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Halcon.Blocks
{
    [Serializable]
    [Block("LineToLineMeasure", BlockCatagory.Measurement)]
    public class LineToLineMeasureBlock : Block
    {
        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public Line Line1 { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public Line Line2 { get; set; }


        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        public AxisOrientation AxisOrientation { get; set; }

        [OutputPort]
        [Browsable(false)]
        [Category(BlockPropertyCategories.Parameter)]
        public Line DistanceLine { get; set; }

        [OutputPort]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category(BlockPropertyCategories.Output)]
        public double Distance { get; set; }

        public override void Process()
        {
            try
            {
                if (Line1 == null || Line2 == null)
                {
                    Status = BlockStatus.Error;
                    Exception = new BlockException("Line1 == null || Line2 == null");
                    return;
                }

                var centerPoint1 = new Point((Line1.X1 + Line1.X2) / 2, (Line1.Y1 + Line1.Y2) / 2);
                var centerPoint2 = new Point((Line2.X1 + Line2.X2) / 2, (Line2.Y1 + Line2.Y2) / 2);

                var vector1 = new Vector(centerPoint1.X, centerPoint1.Y);
                var vector2 = new Vector(centerPoint2.X, centerPoint2.Y);

                Distance = (vector1 - vector2).Length;
                var distanceInXAxis = Math.Abs(vector1.X - vector2.X);
                var distanceInYAxis = Math.Abs(vector1.Y - vector2.Y);

                switch (AxisOrientation)
                {
                    case AxisOrientation.Horizontal:
                        Distance = distanceInXAxis;
                        DistanceLine = new Line(centerPoint1.X, centerPoint1.Y, centerPoint2.X, centerPoint1.Y);
                        break;
                    case AxisOrientation.Vertical:
                        Distance = distanceInYAxis;
                        DistanceLine = new Line(centerPoint1.X, centerPoint1.Y, centerPoint1.X, centerPoint2.Y);
                        break;
                    default:
                        DistanceLine = new Line(centerPoint1.X, centerPoint1.Y, centerPoint2.X, centerPoint2.Y);
                        break;
                }

                Status = BlockStatus.Valid;
                Message = "Process OK";
            }
            catch (Exception e)
            {
                Status = BlockStatus.Error;
                Message = e.Message;
            }
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {            
        }
    }
}
