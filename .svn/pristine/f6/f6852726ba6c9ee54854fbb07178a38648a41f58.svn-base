using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Inspection;
using Hdc.Mv.Production;

namespace Hdc.Mv
{
    [Serializable]
    [ContentProperty("FrameGrabberSchemas")]
    public class HalconMvSchema
    {
        public HalconMvSchema()
        {
            FrameGrabberInitializers = new Collection<IFrameGrabberInitializer>();

            InspectorInitializers = new Collection<IInspectorInitializer>();

            FrameGrabberSchemas = new Collection<HalconFrameGrabberSchema>();
        }

        public Collection<IFrameGrabberInitializer> FrameGrabberInitializers { get; set; }

        public Collection<IInspectorInitializer> InspectorInitializers { get; set; }

        public Collection<HalconFrameGrabberSchema> FrameGrabberSchemas { get; set; }

        public IFrameInfoService FrameInfoService { get; set; }
    }
}