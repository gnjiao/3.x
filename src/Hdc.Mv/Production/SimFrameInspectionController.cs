using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Hdc.Mv.Production;

namespace Hdc.Mv.Inspection
{
    public class SimFrameInspectionController : IFrameInspectionController
    {
        private Subject<FrameDefectInspectionInfo> _frameInspectionStateChangedEvent = new Subject<FrameDefectInspectionInfo>();

        public FrameGrabberSchema FrameGrabberSchema { get; private set; }

        public IObservable<FrameDefectInspectionInfo> FrameInspectionStateChangedEvent
        {
            get { return _frameInspectionStateChangedEvent; }
        }

        public IObservable<IList<FrameInfo>> FrameInfosChangedEvent { get; private set; }

        public void LoadBufferImageInfo()
        {
            throw new NotImplementedException();
        }

        public void LoadImageFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public SimFrameInspectionController()
        {
            
        }

        public void TriggerFrameInspectionStateChangedEvent(FrameDefectInspectionInfo info)
        {
            _frameInspectionStateChangedEvent.OnNext(info);
        }
    }
}