using System;
using System.ComponentModel;
using HalconDotNet;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("HoughCircleFinding", BlockCatagory.Geometry)]
    public class HoughCircleFindingBlock: Block
    {
        public override void Process()
        {
            if (InputRegion == null || InputRegion.CountObj()==0)
            {
                Status = BlockStatus.Error;
                Message = "InputRegion is null";
                Exception = new BlockException(Message);
                return;
            }


            OutputRegion =  InputRegion.HoughCircles(Radius, Percent, Mode);
        }

        [InputPort]
        [Browsable(true)]
        [Category("Parameter")]
        public int Radius { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category("Parameter")]
        [DefaultValue(60)]
        public int Percent { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category("Parameter")]
        public int Mode { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category("Input")]
        public HRegion InputRegion { get; set; }

        [OutputPort]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Output")]
        public HRegion OutputRegion { get; set; }


    }
}