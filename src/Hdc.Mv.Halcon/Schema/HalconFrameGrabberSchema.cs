using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Inspection;

namespace Hdc.Mv
{
    public class HalconFrameGrabberSchema
    {
        public HalconFrameGrabberSchema()
        {
            Plugins = new Collection<IHalconFrameGrabberPlugin>();
        }

        public int Index { get; set; }

        public IFrameGrabber FrameGrabber { get; set; }

        public IHalconInspector Inspector { get; set; }

        public HalconMvSchema MvSchema { get; set; }

        public ICollection<IHalconFrameGrabberPlugin> Plugins { get; set; }

        public ISaveImageFileHalconFrameGrabberPlugin SaveImageFilePlugin { get; set; }

        public IHalconImageCalibrator Calibrator { get; set; }
    }
}