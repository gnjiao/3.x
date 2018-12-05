using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class LineDefinition: DefinitionBase
    {
        public Line ActualLine { get; set; }
        public Line RelativeLine { get; set; }

        public string ReferenceRelativePoint1Name { get; set; }
        public string ReferenceRelativePoint2Name { get; set; }
    }
}