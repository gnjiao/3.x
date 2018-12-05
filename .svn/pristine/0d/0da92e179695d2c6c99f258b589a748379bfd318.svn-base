using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Hdc.Mv.ImageAcquisition;
using Core.Serialization;

namespace Hdc.Mv.Inspection
{
    public class MvController : IMvController
    {
        private readonly Subject<FrameDefectInspectionInfo> _frameInspectionStateChangedEvent = new Subject<FrameDefectInspectionInfo>();
        private IObservable<FrameDefectInspectionInfo> _mergedFrameInspectionStateChangedEvent;

        public MvController()
        {
            FrameInspectionControllers = new List<IFrameInspectionController>();
        }

        public IMvController SetSchema()
        {
            const string fileName = @"MvSchema.xaml";
            var schema = fileName.DeserializeFromXamlFile<MvSchema>();
            SetSchema(schema);
            return this;
        }

        public IMvController SetSchema(MvSchema schema)
        {
            Schema = schema;

            return this;
        }

        public void Initialize()
        {
            foreach (var initializer in Schema.FrameGrabberInitializers)
            {
                initializer.Initialize();
            }

            var cameras = Schema.ChannelSchemas.SelectMany(x => x.FrameGrabberSchemas).Select(x => x.FrameGrabber);
            foreach (var camera in cameras)
            {
                camera.Initialize();
            }

            foreach (var initializer in Schema.InspectorInitializers)
            {
                initializer.Initialize();
            }

            Schema.FrameInfoService.Initialize(Schema);

            for (int i = 0; i < Schema.ChannelSchemas.Count; i++)
            {
                var channelSchema = Schema.ChannelSchemas[i];
                channelSchema.Index = i;
                channelSchema.MvSchema = Schema;

                for (int j = 0; j < channelSchema.FrameGrabberSchemas.Count; j++)
                {
                    var frameGrabberSchema = channelSchema.FrameGrabberSchemas[j];
                    frameGrabberSchema.Index = j;
                    frameGrabberSchema.ChannelSchema = channelSchema;
                    
                    //
                    var sc = new FrameInspectionController(frameGrabberSchema);
                    FrameInspectionControllers.Add(sc);
//                    sc.FrameInspectionStateChangedEvent.Subscribe(_frameInspectionStateChangedEvent);

                    //
                    frameGrabberSchema.SaveImageFilePlugin.Initialize(sc);

                    foreach (var plugin in frameGrabberSchema.Plugins)
                    {
                        plugin.Initialize(sc);
                    }
                }

                _mergedFrameInspectionStateChangedEvent = FrameInspectionControllers.Select(x => x.FrameInspectionStateChangedEvent).Merge();
                _mergedFrameInspectionStateChangedEvent.Subscribe(_frameInspectionStateChangedEvent);
            }
        }

        public void Uninitilaize()
        {
            foreach (var initializer in Schema.FrameGrabberInitializers)
            {
                initializer.Dispose();
            }

            foreach (var initializer in Schema.InspectorInitializers)
            {
                initializer.Dispose();
            }
        }

        public MvSchema Schema { get; private set; }

        public IList<IFrameInspectionController> FrameInspectionControllers { get; private set; }

        public IObservable<FrameDefectInspectionInfo> FrameInspectionStateChangedEvent
        {
            get { return _frameInspectionStateChangedEvent; }
        }
    }
}