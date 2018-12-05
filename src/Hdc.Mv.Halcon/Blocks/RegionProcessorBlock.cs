using System;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("RegionProcessor", BlockCatagory.ImageProcessing)]
    [ContentProperty("RegionProcessor")]
    public class RegionProcessorBlock: Block
    {
        public override void Process()
        {
            if (InputRegion == null)
            {
                Status = BlockStatus.Error;
                Message = "InputRegion is null.";
                Exception = new BlockException("InputRegion is null.");
                return;
            }

            if (RegionProcessor == null)
            {
                Status = BlockStatus.Error;
                Message = "RegionProcessor is null.";
                Exception = new BlockException("RegionProcessor is null.");
                return;
            }

            try
            {
                OutputRegion = RegionProcessor.Process(InputRegion);
                Status = BlockStatus.Valid;
            }
            catch (Exception ex)
            {
                Status = BlockStatus.Error;
                Message = "RegionProcessorBlock Error! RegionProcessor.Process() throw exception.";
                Exception = ex;
            }
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {
        }

        [InputPort]
        public HRegion InputRegion { get; set; }

        [OutputPort]
        public HRegion OutputRegion { get; set; }

        public IRegionProcessor RegionProcessor { get; set; }
    }
}