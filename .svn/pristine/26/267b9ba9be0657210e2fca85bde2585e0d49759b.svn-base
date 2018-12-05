using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class DefectResult : IDisposable
    {
        public DefectDefinition Definition { get; set; }
        public int Index { get; set; }
        public int TypeCode { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Size { get; set; }
        public string Name { get; set; }
        public bool DisplayEnabled { get; set; } = true;
        public bool DisplayHighlight { get; set; } = false;
        public HRegion Region { get; set; }
        public HRegion SmallestRectangle1 { get; set; }
        public double defectArea { get; set; }
        public string Location { get; set; } = "top";
        public void Dispose()
        {
            Region?.Dispose();
            SmallestRectangle1?.Dispose();
        }
    }
}