using System;

namespace Hdc.Mv.ImageAcquisition
{
    public interface IFrameGrabberInitializer: IDisposable
    {
        void Initialize();
    }
}