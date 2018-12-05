using System;
using System.Collections.Generic;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv
{
    public interface ISaveImageFileHalconFrameGrabberPlugin : IHalconFrameGrabberPlugin
    {
        IObservable<HalconImageFileSavedResult> ImageFileSavedEvent { get; }

        IList<HalconImageFileSavedResult> GetImageFileSavedResults();

        bool SaveImageEnabled { get; set; }
    }
}