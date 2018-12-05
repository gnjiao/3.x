using System.Collections.Generic;
using System.Linq;

namespace Hdc.Mv.Inspection
{
    public static class InspectionResultExtensions
    {
        public static IList<DefectResult> GetDefectResults(this InspectionResult inspectionResult)
        {
            return inspectionResult.RegionDefectResults.SelectMany(y => y.DefectResults).ToList();
        }
    }
}