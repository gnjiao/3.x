using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Inspection;

namespace Hdc.Mv
{
    [ContentProperty("DefectInfos")]
    public class SimDefectHalconInspector : IHalconInspector
    {
        private int counter;

        public SimDefectHalconInspector()
        {
            DefectInfos = new Collection<DefectInfo>();
            InspectionResults = new Collection<bool>();
        }

        public int Index { get; set; }

        public string Name { get; set; }

        public InspectionResult Inspect(HImage imageInfo)
        {
            var InspectionResult = new InspectionResult();
//            InspectionResult.RegionDefectResults

            Thread.Sleep(ProcessTime);

            var offset = counter%InspectionResults.Count;
            var result = InspectionResults[offset];

            counter++;

            if (result)
                return InspectionResult;
            else
            {
                var regionDefectResult = new RegionDefectResult();
                InspectionResult.RegionDefectResults.Add(regionDefectResult);
                foreach (var defectInfo in DefectInfos)
                {
                    var hRegion = new HRegion();
                    hRegion.GenRectangle2(defectInfo.Y, defectInfo.X, 0, defectInfo.Width, defectInfo.Height);

                    var defectResult = defectInfo.ToDefectResult();
//                    defectResult.Name = regionDefectResult.RegionResult.SurfaceName;
                    defectResult.Region = hRegion;
                    regionDefectResult.DefectResults.Add(defectResult);
                }

                return InspectionResult;
            }
        }

        /// <summary>
        /// millisecondsTimeout
        /// </summary>
        public int ProcessTime { get; set; }

        public int MaxDefectCount { get; set; }

        public void Dispose()
        {
        }

        public Collection<DefectInfo> DefectInfos { get; set; }

        public Collection<bool> InspectionResults { get; set; }
    }
}