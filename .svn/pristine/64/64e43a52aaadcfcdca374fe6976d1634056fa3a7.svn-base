using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using Core.Collections.Generic;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("FrameGrabberSchemas")]
    public class ChannelSchema
    {
        public ChannelSchema()
        {
            FrameGrabberSchemas = new Collection<FrameGrabberSchema>();
        }

        public ChannelSchema(params FrameGrabberSchema[] frameGrabberSchemata) : this()
        {
            FrameGrabberSchemas.AddRange(frameGrabberSchemata);
        }

        public ChannelSchema(int index, string name, params FrameGrabberSchema[] frameGrabberSchemata) : this()
        {
            Index = index;
            Name = name;
            FrameGrabberSchemas.AddRange(frameGrabberSchemata);
        }

        public MvSchema MvSchema { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public Collection<FrameGrabberSchema> FrameGrabberSchemas { get; set; }
    }
}