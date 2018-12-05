using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class XldSearchingDefinition: DefinitionBase
    {
        public IXldExtractor XldExtractor { get; set; }

        public Line RoiActualLine { get; set; }
        public double RoiHalfWidth { get; set; }
        public Line RoiRelativeLine { get; set; }
    }
}