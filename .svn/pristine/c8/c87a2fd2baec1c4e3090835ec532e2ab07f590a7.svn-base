using System;
using System.Collections.Generic;

namespace Hdc.Mv.Inspection
{
    public interface IGeneralDefectInspector : IDisposable
    {
        int Index { get; set; }

        string Name { get; set; }

        IList<DefectInfo> Inspect(ImageInfo imageInfo);

        int MaxDefectCount { get; set; }
    }
}