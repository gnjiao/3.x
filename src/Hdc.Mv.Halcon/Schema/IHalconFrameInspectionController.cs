using System;
using Hdc.Mv.Inspection;

namespace Hdc.Mv
{
    public interface IHalconFrameInspectionController
    {
        HalconFrameGrabberSchema FrameGrabberSchema { get; }

        IObservable<HalconFrameInspectionInfo> FrameInspectionStateChangedEvent { get; }

        void LoadBufferImageInfo();

        void LoadImageFile(string fileName);

//        IFrameInfoController FrameInfoController { get; }
    }
}