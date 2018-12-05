using System.Collections.Generic;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Production;

namespace Hdc.Mv.Inspection
{
    public class FrameDefectInspectionInfo
    {
        public FrameInfo FrameInfo { get; set; }
        public IList<DefectInfo> DefectInfos { get; set; }

        public FrameDefectInspectionInfo()
        {
            DefectInfos = new List<DefectInfo>();
        }

        public FrameDefectInspectionInfo(FrameInfo frameInfo): this()
        {
            FrameInfo = frameInfo;
        }

        public FrameDefectInspectionInfo(FrameInfo frameInfo, IList<DefectInfo> defectInfos)
        {
            FrameInfo = frameInfo;
            DefectInfos = defectInfos;
        }

        public FrameState State
        {
            get { return FrameInfo.State; }
        }
    }
}