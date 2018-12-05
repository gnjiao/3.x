using System;
using Hdc.Mv.Production;

namespace Hdc.Mv.ImageAcquisition
{
    public interface IFrameGrabber
    {
        int Index { get; set; }

        string Name { get; set; }

        void Initialize();

        IObservable<GrabInfo> GrabStateChangedEvent { get; }

        void Trigger();

        void Trigger(Action beforeTriggerAction);

        int Tag { get; }
    }
}