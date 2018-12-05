using System;
using System.Collections.Generic;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Production;

namespace Hdc.Mv.Inspection
{
    public interface IFrameInfoController
    {
        IObservable<FrameInfo> FrameStateChangedEvent { get; }

        void LoadImageFile(string fileName);

        void LoadBufferImageInfo();

        FrameInfo BufferSurfaceImageInfo { get; }
    }
}