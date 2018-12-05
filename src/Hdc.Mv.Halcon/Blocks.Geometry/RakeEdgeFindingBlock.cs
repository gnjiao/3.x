using System;
using System.ComponentModel;
using System.Linq;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("RakeEdgeFinding", BlockCatagory.Position)]
    public class RakeEdgeFindingBlock : RegionOfInterestBlock
    {
        // General
        [InputPort]
        [Browsable(false)]
        [Category(BlockPropertyCategories.Roi)]
        [DefaultValue(0.0)]
        public double StartX { get; set; }

        [InputPort]
        [Browsable(false)]
        [Category(BlockPropertyCategories.Roi)]
        public double StartY { get; set; }

        [InputPort]
        [Browsable(false)]
        [Category(BlockPropertyCategories.Roi)]
        public double EndX { get; set; }

        [InputPort]
        [Browsable(false)]
        [Category(BlockPropertyCategories.Roi)]
        public double EndY { get; set; }

        [InputPort]
        [Browsable(false)]
        [Category(BlockPropertyCategories.Roi)]
        public double RoiHalfWidth { get; set; }

        //     
        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        public Orientation Orientation { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]

        public int RegionsCount { get; set; }

        [InputPort]
        [Browsable(false)]
        [Category(BlockPropertyCategories.Parameter)]
        [DefaultValue(50)]
        public int RegionHeight { get; set; }

        [InputPort]
        [Browsable(false)]
        [Category(BlockPropertyCategories.Parameter)]
        [DefaultValue(200)]
        public int RegionWidth { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [DefaultValue(20)]
        public double Sigma { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [DefaultValue(1)]
        public double Threshold { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [DefaultValue(10.0)]
        //        [DefaultValue(Mv.SelectionMode.First)]
        public SelectionMode SelectionMode { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [DefaultValue("First")]
        public Transition Transition { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        [DefaultValue("all")]
        public HImage Image { get; set; }        

        [OutputPort]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category(BlockPropertyCategories.Output)]
        public Line Line { get; set; }        

        public override void Process()
        {
            if (Image == null || Image.Key == IntPtr.Zero)
            {
                Status = BlockStatus.Error;
                Exception = new BlockException("Image == null || Image.Key == IntPtr.Zero");
                return;
            }

            if (Roi?.RoiRegion != null && Roi?.RoiType == RegionOfInterestType.rectangle1)
            {
                StartX = Roi.Column1 ;
                StartY = Roi.Row1;

                EndX = Roi.Column2;
                EndY = Roi.Row2;
                
                RegionWidth = (int)Math.Abs(Roi.Column2 - Roi.Column1); 
                RegionHeight = (int)Math.Abs(Roi.Row2 - Roi.Row1);                
            }
            else
            {
                
                return;
            }

            try
            {


                var searchLine = Orientation == Orientation.Vertical ? 
                    new Line(StartX, StartY+ (double)RegionHeight / 2, EndX, EndY- (double)RegionHeight / 2)
                    : new Line(StartX + (double)RegionWidth / 2, StartY , EndX - (double)RegionWidth / 2, EndY);

                var lines = HDevelopExport.Singletone.RakeEdgeLine(Image,
                    line: searchLine,
                    regionsCount: RegionsCount,
                    regionHeight: RegionHeight,
                    regionWidth: RegionWidth,
                    sigma: Sigma,
                    threshold: Threshold,
                    transition: Transition,
                    selectionMode: SelectionMode);

                if (lines.Any())
                    Line = lines.First();


                //Line = searchLine;

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