using System;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Production
{
    [Serializable]
    public class FrameInfo
    {
        // FrameInfoService
        public int WorkpieceTag { get; set; } // running tag of a workpiece. from FrameInfoService.
        public int SurfaceTag { get; set; } // running tag of a Surface. from FrameInfoService

        public int WorkpieceIndex { get; set; } // index of workpiece of a channel. from FrameInfoService.
        public int SurfaceIndex { get; set; } // index of surface of a workpiece. from FrameInfoService.

        // FrameGrabber
        public int FrameTag { get; set; } // running tag of a Frame. from FrameGrabber

        // schema
        public int ChannelIndex { get; set; } // channel index from schema
        public int FrameGrabberIndex { get; set; } // frameGrabber index from schema
        public string ChannelName { get; set; }
        public string FrameGrabberName { get; set; }

        public FrameState State { get; set; }
        public ImageInfo ImageInfo { get; set; }
    }
}