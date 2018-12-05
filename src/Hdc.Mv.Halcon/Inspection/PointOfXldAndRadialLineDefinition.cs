using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class PointOfXldAndRadialLineDefinition : DefinitionBase
    {
        public string XldName { get; set; }
        public string RadialLineName { get; set; }
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Last;
        public double StandardValue { get; set; }
    }
}