using System;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Production
{
    public interface IFrameInfoService
    {
        void Initialize(MvSchema schema);

        FrameInfo GetFrameInfo(int channelIndex, int frameGrabberIndex);

        FrameInfo GetFrameInfo(int surfaceTag);

        void ChangeFrameState(int frameGrabberIndex, int surfaceTag, FrameState frameState);

        void Step();

        IObservable<int> SurfaceTagChangedEvent { get; }
    }
}