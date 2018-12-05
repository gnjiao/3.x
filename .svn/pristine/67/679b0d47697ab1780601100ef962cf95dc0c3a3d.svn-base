using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Production;

namespace Hdc.Mv.Inspection
{
    public interface IFrameInspectionController
    {
        FrameGrabberSchema FrameGrabberSchema { get; }

        IObservable<FrameDefectInspectionInfo> FrameInspectionStateChangedEvent { get; }

        IObservable<IList<FrameInfo>> FrameInfosChangedEvent { get; }

        void LoadBufferImageInfo();
        void LoadImageFile(string fileName);
    }
}