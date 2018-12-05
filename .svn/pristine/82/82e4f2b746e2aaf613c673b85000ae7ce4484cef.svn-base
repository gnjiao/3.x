using System;
using System.Linq;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class StepFromGrayValueInspector : IStepFromGrayValueInspector
    {
        public StepFromGrayValueResult Calculate(
            HImage image, 
            StepFromGrayValueDefinition definition, 
            InspectionResult inspectionResult)
        {
            var regionResult1 = inspectionResult.RegionSearchingResults.FirstOrDefault(
                x => x.Definition.Name == definition.RegionName1);

            var regionResult2 = inspectionResult.RegionSearchingResults.FirstOrDefault(
                x => x.Definition.Name == definition.RegionName2);

            if (regionResult1 == null || regionResult2 == null)
            {
                return new StepFromGrayValueResult() {StepValueInGrayValue = -999.999};
            }

            double dev1, dev2;
            var gray1 = image.Intensity(regionResult1.Region, out dev1);
            var gray2 = image.Intensity(regionResult2.Region, out dev2);

            var height1 = gray1*definition.ZScaleInMillimeter;
            var height2 = gray2*definition.ZScaleInMillimeter;

            var result = new StepFromGrayValueResult()
            {
                Definition = definition,
                StepValueInGrayValue = gray1 - gray2,
                StepValueInMillimeter = height1 - height2,
            };

            return result;
        }
    }
}