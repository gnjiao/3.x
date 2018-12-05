using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class StepFromGrayValueDefinition: DefinitionBase
    {
        public string RegionName1 { get; set; }
        public string RegionName2 { get; set; }
        public double StandardValue { get; set; }
    }
}