using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hdc.Mv.Inspection
{
    public static class DefectInspectorExtensions
    {
        public static Task<IList<DefectInfo>> InspectAsync(this IGeneralDefectInspector inspector,
                                                                 ImageInfo imageInfo)
        {
            return Task.Run(() => inspector.Inspect(imageInfo));
        }
    }
}