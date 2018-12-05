using System;
using System.Collections.ObjectModel;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Production;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MvSchema
    {
        public MvSchema()
        {
            FrameGrabberInitializers = new Collection<IFrameGrabberInitializer>();

            InspectorInitializers = new Collection<IInspectorInitializer>();

            ChannelSchemas = new Collection<ChannelSchema>();
        }

        public Collection<IFrameGrabberInitializer> FrameGrabberInitializers { get; set; }

        public Collection<IInspectorInitializer> InspectorInitializers { get; set; }

        public Collection<ChannelSchema> ChannelSchemas { get; set; }

        public IFrameInfoService FrameInfoService { get; set; }
    }
}