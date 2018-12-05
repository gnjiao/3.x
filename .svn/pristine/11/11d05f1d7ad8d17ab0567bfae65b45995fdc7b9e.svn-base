using System.Threading.Tasks;
using HalconDotNet;
using Hdc.Mv.Inspection;

namespace Hdc.Mv
{
    public static class HalconInspectorExtensions
    {
        public static Task<InspectionResult> InspectAsync(this IHalconInspector halconInspector, HImage imageInfo)
        {
            return Task.Run(() => halconInspector.Inspect(imageInfo));
        }
    }
}