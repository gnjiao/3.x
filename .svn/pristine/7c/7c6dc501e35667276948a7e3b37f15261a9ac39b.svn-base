using System;
using System.Collections.Generic;
using System.ComponentModel;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;

namespace Hdc.Mv.Halcon
{
    [Block("SpokeCircleFinding", BlockCatagory.Position)]
    public class SpokeCircleFindingBlock : RegionOfInterestBlock
    {
        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public double Row { get; set; } = 500;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public double Column { get; set; } = 500;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public double InnerRadius { get; set; } = 50;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public double OuterRadius { get; set; } = 100;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public int RegionsCount { get; set; } = 50;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public int RegionWidth { get; set; } = 50;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public double Sigma { get; set; } = 1;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public double Threshold { get; set; } = 20;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.First;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public Transition Transition { get; set; } = Transition.All;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public CircleDirect Direct { get; set; } = CircleDirect.Inner;

        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public string EllipseMode { get; set; } = "Circle";

        /// <summary>
        /// List of values: 'fhuber', 'fitzgibbon', 'focpoints', 'fphuber', 'fptukey', 'ftukey', 'geohuber', 'geometric', 'geotukey', 'voss'
        /// </summary>
        [Category(BlockPropertyCategories.InputControl)]
        [InputPort]
        public string EllipseAlgorithm { get; set; } = "fitzgibbon";

        [Category(BlockPropertyCategories.OutputObject)]
        [OutputPort]
        public Circle Circle { get; set; }

        [Category(BlockPropertyCategories.Input)]
        [InputPort]
        public HImage Image { get; set; }

        [Category(BlockPropertyCategories.Output)]
        [OutputPort]
        [Browsable(false)]
        public List<HObject> Crosses { get; set; }


        public override void Process()
        {
            if (Image == null || Image.Key == IntPtr.Zero)
            {
                Status = BlockStatus.Error;
                Exception = new BlockException("Image == null || Image.Key == IntPtr.Zero");
                return;
            }

            if (Roi?.RoiRegion != null && Roi?.RoiType == RegionOfInterestType.circle)
            {
                Row = Roi.Row;
                Column = Roi.Column;
                InnerRadius = Roi.Radius - RegionWidth;
                OuterRadius = Roi.Radius + RegionWidth;
            }
            else
            {
                return;
            }

            try
            {
                var isOk = HDevelopExport.Singletone.ExtractCircle(
                    Image,
                    Row,
                    Column,
                    InnerRadius,
                    OuterRadius,
                    out var foundCircle,
                    out var roundless,
                    RegionsCount,
                    RegionWidth,
                    Sigma,
                    Threshold,
                    SelectionMode,
                    Transition,
                    Direct,
                    EllipseMode,
                    EllipseAlgorithm
                    );

                if (isOk)
                {
                    Circle = new Circle(
                        foundCircle.CenterX,
                        foundCircle.CenterY,
                        foundCircle.Radius);
                    Status = BlockStatus.Valid;
                    return;
                }
                else
                {
                    Status = BlockStatus.Error;
                    Exception = new BlockException("ExtractCircle error. ");
                    return;
                }
            }
            catch (Exception ex)
            {
                Status = BlockStatus.Error;
                Exception = new BlockException("ExtractCircle error. ", ex);
                return;
            }
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {
        }
    }
}