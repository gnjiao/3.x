using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using Core;
using Core.Collections;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Production;
using Core.Reactive.Linq;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv.Inspection
{
    public class FrameInspectionController : IFrameInspectionController
    {
        private readonly IGeneralDefectInspector _defectInspector;
        private readonly FrameGrabberSchema _frameGrabberSchema;

        private readonly ConcurrentDictionary<long, FrameInfo> _frameInfos =
            new ConcurrentDictionary<long, FrameInfo>();

        private readonly Subject<IList<FrameInfo>> _frameInfosChangedEvent =
            new Subject<IList<FrameInfo>>();

        private readonly IFrameInfoController _frameInfoController;

        private readonly Subject<FrameDefectInspectionInfo> _frameInspectionStateChangedEvent =
            new Subject<FrameDefectInspectionInfo>();

        public FrameInspectionController(FrameGrabberSchema frameGrabberSchema)
        {
            _defectInspector = frameGrabberSchema.Inspector;
            _frameGrabberSchema = frameGrabberSchema;

            var channelIndex = frameGrabberSchema.ChannelSchema.Index;
            var cameraIndex = frameGrabberSchema.Index;

            _frameInfoController = new FrameInfoController(channelIndex,
                cameraIndex,
                frameGrabberSchema.ChannelSchema.MvSchema.FrameInfoService,
                frameGrabberSchema.FrameGrabber);

            _frameInfoController.FrameStateChangedEvent
//                .ObserveOnTaskPool()
                .Subscribe(async x =>
                {
//                              Debug.WriteLine("CameraController 00::GlobalFrameTag: {0}, CameraFrameTag: {1}, State: {2}",
//                                  xx.FrameInfo.GlobalFrameTag,
//                                  xx.FrameInfo.CameraFrameTag,
//                                  xx.FrameInfo.State);
                    var fi = x.DeepClone();
                    switch (fi.State)
                    {
                        case FrameState.Grabbing:
                            _frameInfos.TryAdd(x.FrameTag, fi);
                            _frameInspectionStateChangedEvent.OnNext(new FrameDefectInspectionInfo(fi));
                            Refresh();
                            break;
                        case FrameState.Grabbed:
                            var ssi = _frameInfos[fi.FrameTag];
                            ssi.ImageInfo = fi.ImageInfo;
                            ssi.State = FrameState.Grabbed;
                            _frameInspectionStateChangedEvent.OnNext(new FrameDefectInspectionInfo(ssi.DeepClone()));
                            Refresh();

                            ssi.State = FrameState.Inspecting;
                            _frameInspectionStateChangedEvent.OnNext(new FrameDefectInspectionInfo(ssi.DeepClone()));
                            Refresh();

                            //
                            Debug.WriteLine("FrameInspectionController::_defectInspector.InspectAsync begin: " +
                                            "SurfaceTag={0}, " +
                                            "ChannelIndex={1}, " +
                                            "FrameTag={2}, " +
                                            "FrameGrabberIndex={3}, " +
                                            "State={4}, " + 
                                            "DateTime={5}",
                                ssi.SurfaceTag,
                                ssi.ChannelIndex,
                                ssi.FrameTag,
                                ssi.FrameGrabberIndex,
                                ssi.State,
                                DateTime.Now);

                            var ds = await _defectInspector.InspectAsync(ssi.ImageInfo);

                            Debug.WriteLine("FrameInspectionController::_defectInspector.InspectAsync end: " +
                                            "SurfaceTag={0}, " +
                                            "ChannelIndex={1}, " +
                                            "FrameTag={2}, " +
                                            "FrameGrabberIndex={3}, " +
                                            "State={4}, " +
                                            "DefectCount={5}" +
                                            "DateTime={6}",
                                ssi.SurfaceTag,
                                ssi.ChannelIndex,
                                ssi.FrameTag,
                                ssi.FrameGrabberIndex,
                                ssi.State,
                                ds.Count,
                                DateTime.Now);

                            if (_defectInspector.MaxDefectCount > 0)
                            {
                                if (ds.Count > _defectInspector.MaxDefectCount)
                                {
                                    ds = ds.Take(_defectInspector.MaxDefectCount).ToList();
                                }
                            }
                            ssi.State = FrameState.Inspected;

                            var frameDefectInspectionInfo = new FrameDefectInspectionInfo(ssi.DeepClone(), ds);
                            _frameInspectionStateChangedEvent.OnNext(frameDefectInspectionInfo);

//                            Debug.WriteLine("FrameInspectionController::FrameState.Inspected: " +
//                                            "SurfaceTag={0}, " +
//                                            "ChannelIndex={1}, " +
//                                            "FrameTag={2}, " +
//                                            "FrameGrabberIndex={3}, " +
//                                            "State={4}, " +
//                                            "DefectCount={5}",
//                                ssi.SurfaceTag,
//                                ssi.ChannelIndex,
//                                ssi.FrameTag,
//                                ssi.FrameGrabberIndex,
//                                ssi.State,
//                                frameDefectInspectionInfo.DefectInfos.Count);
                            Refresh();
                            break;
                    }
                });
        }

        public IFrameGrabber FrameGrabber
        {
            get { return _frameGrabberSchema.FrameGrabber; }
        }

        public FrameGrabberSchema FrameGrabberSchema
        {
            get { return _frameGrabberSchema; }
        }

        public IObservable<FrameDefectInspectionInfo> FrameInspectionStateChangedEvent
        {
            get { return _frameInspectionStateChangedEvent; }
        }

        public IGeneralDefectInspector DefectInspector
        {
            get { return _defectInspector; }
        }

        public void LoadBufferImageInfo()
        {
            _frameInfoController.LoadBufferImageInfo();
        }

        public void LoadImageFile(string fileName)
        {
            _frameInfoController.LoadImageFile(fileName);
        }

        public IObservable<IList<FrameInfo>> FrameInfosChangedEvent
        {
            get { return _frameInfosChangedEvent; }
        }

        private void Refresh()
        {
            _frameInfosChangedEvent.OnNext(_frameInfos.Values.ToList());
        }
    }
}