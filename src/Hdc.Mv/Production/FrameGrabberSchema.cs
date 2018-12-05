using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Production;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class FrameGrabberSchema
    {
        public FrameGrabberSchema()
        {
            Plugins = new Collection<IFrameGrabberPlugin>();
        }

        public int Index { get; set; }

        public IFrameGrabber FrameGrabber { get; set; }

        public IGeneralDefectInspector Inspector { get; set; }

        public ChannelSchema ChannelSchema { get; set; }

        public ICollection<IFrameGrabberPlugin> Plugins { get; set; }

        public ISaveImageFilePlugin SaveImageFilePlugin { get; set; }
    }
}