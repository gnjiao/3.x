using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Inspection;

namespace Hdc.Mv
{
    [ContentProperty("DefectInfos")]
    public class SimDefectLiteHalconInspector : IHalconInspector
    {
#pragma warning disable 169
        private int counter;
#pragma warning restore 169

        public SimDefectLiteHalconInspector()
        {
            DefectInfos = new Collection<DefectInfo>();
        }

        public int Index { get; set; }

        public string Name { get; set; }

        public InspectionResult Inspect(HImage imageInfo)
        {
            var InspectionResult = new InspectionResult();
            var regionDefectResult = new RegionDefectResult();
            InspectionResult.RegionDefectResults.Add(regionDefectResult);
            foreach (var defectInfo in DefectInfos)
            {
                var hRegion = new HRegion();
                hRegion.GenRectangle2(defectInfo.Y, defectInfo.X, 0, defectInfo.Width, defectInfo.Height);

                var defectResult = defectInfo.ToDefectResult();
//                defectResult.Name = regionDefectResult.RegionResult.SurfaceName;
                defectResult.Region = hRegion;
                regionDefectResult.DefectResults.Add(defectResult);
            }

            InspectionResult.Coordinate = new MockRelativeCoordinate();

            return InspectionResult;
        }

        public void Dispose()
        {
        }

        public Collection<DefectInfo> DefectInfos { get; set; }
    }
}