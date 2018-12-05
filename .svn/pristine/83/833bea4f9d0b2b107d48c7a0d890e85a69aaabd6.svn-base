using System;
using System.Collections.Generic;

namespace Hdc.Mv.Inspection
{
    public interface ISaveImageFilePlugin : IFrameGrabberPlugin
    {
        IObservable<ImageFileSavedResult> ImageFileSavedEvent { get; }

        IList<ImageFileSavedResult> GetImageFileSavedResults();

        bool SaveImageEnabled { get; set; }
    }
}