using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Hdc.Mv.Inspection;
using Core.Serialization;

namespace Hdc.Mv
{
    public class HalconMvController
    {
        private readonly Subject<HalconFrameInspectionInfo> _frameInspectionStateChangedEvent =
            new Subject<HalconFrameInspectionInfo>();

#pragma warning disable 169
        private IObservable<HalconFrameInspectionInfo> _mergedFrameInspectionStateChangedEvent;
#pragma warning restore 169

        public HalconMvController()
        {
            FrameInspectionControllers = new List<IHalconFrameInspectionController>();
        }

        public HalconMvController SetSchema()
        {
            const string fileName = @"HalconMvSchema.xaml";
            var schema = fileName.DeserializeFromXamlFile<HalconMvSchema>();
            SetSchema(schema);
            return this;
        }

        public HalconMvController SetSchema(HalconMvSchema schema)
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

            var cameras = Schema.FrameGrabberSchemas.Select(x => x.FrameGrabber);
            foreach (var camera in cameras)
            {
                camera.Initialize();
            }

            foreach (var initializer in Schema.InspectorInitializers)
            {
                initializer.Initialize();
            }

            Schema.FrameInfoService.Initialize(null);

            for (int j = 0; j < Schema.FrameGrabberSchemas.Count; j++)
            {
                var frameGrabberSchema = Schema.FrameGrabberSchemas[j];
                frameGrabberSchema.Index = j;
                frameGrabberSchema.MvSchema = Schema;

                //
                var sc = new HalconFrameInspectionController(frameGrabberSchema);
                FrameInspectionControllers.Add(sc);

                //
                frameGrabberSchema.SaveImageFilePlugin.Initialize(sc);

                foreach (var plugin in frameGrabberSchema.Plugins)
                {
                    plugin.Initialize(sc);
                }

                sc.FrameInspectionStateChangedEvent.Subscribe(
                    x =>
                    {
                        Debug.WriteLine("HalconMvController.FrameInspectionStateChangedEvent begin, at " + DateTime.Now);
                        Debug.WriteLine("HalconMvController.FrameInspectionStateChangedEvent.OnNext begin, at " +
                                        DateTime.Now);
                        _frameInspectionStateChangedEvent.OnNext(x);
                        Debug.WriteLine("HalconMvController.FrameInspectionStateChangedEvent.OnNext end, at " +
                                        DateTime.Now);
                        Debug.WriteLine("HalconMvController.FrameInspectionStateChangedEvent end, at " + DateTime.Now);
                    });
            }

//            _mergedFrameInspectionStateChangedEvent =
//                FrameInspectionControllers.Select(x => x.FrameInspectionStateChangedEvent).Merge();
//            _mergedFrameInspectionStateChangedEvent.Subscribe(_frameInspectionStateChangedEvent);
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

        public HalconMvSchema Schema { get; private set; }

        public IList<IHalconFrameInspectionController> FrameInspectionControllers { get; private set; }

        public IObservable<HalconFrameInspectionInfo> FrameInspectionStateChangedEvent
        {
            get { return _frameInspectionStateChangedEvent; }
        }
    }
}