using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class DistanceBetweenPointsOfXldAndRadialLineDefinition : DefinitionBase
    {
        public string Point1OfXldAndRadialLineName { get; set; }
        public string Point2OfXldAndRadialLineName { get; set; }
        
        public double StandardValue { get; set; }


//        public string Point1XldName { get; set; }
//        public string Point1RadialLineName { get; set; }
//        public SelectionMode Point1SelectionMode { get; set; } = SelectionMode.Last;
//        public double Point1StandardValue { get; set; }
//
//        public string Point2XldName { get; set; }
//        public string Point2RadialLineName { get; set; }
//        public SelectionMode Point2SelectionMode { get; set; } = SelectionMode.Last;
//        public double Point2StandardValue { get; set; }
    }
}