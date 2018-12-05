using System;
using System.Windows.Markup;

// ReSharper disable InconsistentNaming

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class RegionSearchingDefinition : DefinitionBase
    {
        public Line RoiActualLine { get; set; }
        public Line RoiRelativeLine { get; set; }
        public double RoiHalfWidth { get; set; }

        public IRegionExtractor RegionExtractor { get; set; }

        /// <summary>
        /// Z offset use for creating base plane.
        /// </summary>
        public double ZOffsetInMillimeter { get; set; }
    }
}

// ReSharper restore InconsistentNaming