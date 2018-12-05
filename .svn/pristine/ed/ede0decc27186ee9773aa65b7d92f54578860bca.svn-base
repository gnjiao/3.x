using HalconDotNet;
using Hdc.Mv.Inspection;
using Hdc.Mv.Production;

namespace Hdc.Mv
{
    public class HalconFrameInspectionInfo
    {
        public FrameInfo FrameInfo { get; set; }

        public HImage CalibratedImage { get; set; }

        public InspectionResult InspectionResult { get; set; }

        public FrameState State
        {
            get { return FrameInfo.State; }
            set { FrameInfo.State = value; }
        }

        public HalconFrameInspectionInfo()
        {
        }

        public HalconFrameInspectionInfo(FrameInfo frameInfo)
            : this()
        {
            FrameInfo = frameInfo;
        }

        public IRelativeCoordinate Coordinate { get; set; }

        public object Payload { get; set; }

        public bool CalibrationDisabled { get; set; }
    }
}