using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Markup;

namespace Hdc.Mv.Inspection
{
    [ContentProperty("DefectInfos")]
    public class SimDefectInspector : IGeneralDefectInspector
    {
        private int counter;

        public SimDefectInspector()
        {
            DefectInfos = new Collection<DefectInfo>();
            InspectionResults = new Collection<bool>();
        }

        public int Index { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// millisecondsTimeout
        /// </summary>
        public int ProcessTime { get; set; }

        public IList<DefectInfo> Inspect(ImageInfo imageInfo)
        {
            Thread.Sleep(ProcessTime);

            var offset = counter%InspectionResults.Count;
            var result = InspectionResults[offset];

            counter++;

            if (result)
                return new List<DefectInfo>();
            else
                return DefectInfos;
        }

        public int MaxDefectCount { get; set; }

        public void Dispose()
        {
        }

        public Collection<DefectInfo> DefectInfos { get; set; }

        public Collection<bool> InspectionResults { get; set; }
    }
}