using System;
using HalconDotNet;

namespace Hdc.Mv.ImageAcquisition
{
    public interface IHalconCamera : IDisposable
    {
        void Initialize();

        HImage Acquisition();
    }
}