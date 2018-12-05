using System;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    public class PortReference
    {
        public string TargetPortName { get; set; }

        public string SourceBlockName { get; set; } 

        public string SourcePortName { get; set; }

        public PortReference()
        {
        }

        public PortReference(string targetPortName, string sourceBlockName = null, string sourcePortName=null)
        {
            SourceBlockName = sourceBlockName;
            SourcePortName = sourcePortName;
            TargetPortName = targetPortName;
        }
    }
}