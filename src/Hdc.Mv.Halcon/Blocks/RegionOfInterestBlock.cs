using System;
using System.ComponentModel;
using HalconDotNet;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;
using Hdc.Mv.PropertyItem.Controls;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class RegionOfInterestBlock : Block
    {

        [Browsable(true)]
        [Category(BlockPropertyCategories.Common)]
        [Editor(typeof(RoiControlEditor), typeof(RoiControlEditor))]
        public RegionOfInterest Roi { get; set; }

        public abstract override void Process();
        
    }
}
