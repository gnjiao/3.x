using System;
using System.Collections.Generic;
using Hdc.Mv.Inspection;

namespace Hdc.Mv
{
    public interface IMvController
    {
        IMvController SetSchema(MvSchema schema);
        MvSchema Schema { get; }
        void Initialize();
        IList<IFrameInspectionController> FrameInspectionControllers { get; }
        IObservable<FrameDefectInspectionInfo> FrameInspectionStateChangedEvent { get; }
    }
}