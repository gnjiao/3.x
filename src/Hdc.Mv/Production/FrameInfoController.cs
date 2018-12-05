using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Core;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Production;
using Core.Reactive;
using Core.Reactive.Linq;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv.Inspection
{
    public class FrameInfoController : IFrameInfoController
    {
        private readonly int _channelIndex;
        private readonly int _cameraIndex;
        private readonly IFrameGrabber _camera;
        private readonly IFrameInfoService _frameInfoService;
        private readonly Subject<FrameInfo> _frameStateChangedEvent = new Subject<FrameInfo>();

        private readonly ConcurrentDictionary<int, FrameInfo> _surfaceImageInfos =
            new ConcurrentDictionary<int, FrameInfo>();

        public FrameInfoController(int channelIndex,
                                   int cameraIndex,
                                   IFrameInfoService frameInfoService,
                                   IFrameGrabber camera)
        {
            _channelIndex = channelIndex;
            _cameraIndex = cameraIndex;
            _camera = camera;
            _frameInfoService = frameInfoService;

            _camera.GrabStateChangedEvent
//                                .ObserveOnTaskPool()
//                .ObserveOn(_synchronizationContext)
                .Subscribe(
                    x =>
                    {
//                        Debug.WriteLine("FrameInfoController._camera.GrabStateChangedEvent.Subscribe, " + x.State);
                        switch (x.State)
                        {
                            case GrabState.Started:
                                Debug.WriteLine("FrameInfoController.GrabState.Started begin, at " + DateTime.Now);
//                                    Debug.WriteLine("FrameInfoController.GetFrameInfo begin");
                                var sii = GetFrameInfo(x);
                                if (sii.SurfaceTag == -255)
                                    return;
//                                    Debug.WriteLine("FrameInfoController.GetFrameInfo begin");
                                sii.State = FrameState.Grabbing;

//                                    Debug.WriteLine("FrameInfoController._surfaceImageInfos.TryAdd begin");
                                var ret = _surfaceImageInfos.TryAdd(sii.FrameTag, sii);
//                                    Debug.WriteLine("FrameInfoController._surfaceImageInfos.TryAdd end");

                                if (!ret)
                                    throw new Exception("SurfaceImageInfosByCameraFrameTag.TryAdd()");
                                BufferSurfaceImageInfo = sii;
                                Debug.WriteLine("FrameInfoController.GrabState.Started.OnNext(Grabbing) begin, at " +
                                                DateTime.Now);
                                _frameStateChangedEvent.OnNext(sii.DeepClone());
                                Debug.WriteLine("FrameInfoController.GrabState.Started.OnNext(Grabbing) end, at " +
                                                DateTime.Now);
                                Debug.WriteLine("FrameInfoController.GrabState.Started end, at " + DateTime.Now);
                                break;
                            case GrabState.Completed:
                                Debug.WriteLine("FrameInfoController.GrabState.Completed begin, at " + DateTime.Now);
                                var sii2 = _surfaceImageInfos[x.FrameTag];
                                sii2.ImageInfo = x.ImageInfo;
                                sii2.State = FrameState.Grabbed;

                                BufferSurfaceImageInfo = sii2;
                                Debug.WriteLine("FrameInfoController.GrabState.Started.OnNext(Grabbed) begin, at " +
                                                DateTime.Now);
                                _frameStateChangedEvent.OnNext(sii2.DeepClone());
                                Debug.WriteLine("FrameInfoController.GrabState.Started.OnNext(Grabbed) end, at " +
                                                DateTime.Now);
                                Debug.WriteLine("FrameInfoController.GrabState.Completed end, at " + DateTime.Now);
                                break;
                        }
                    });
        }

        private FrameInfo GetFrameInfo(GrabInfo grabInfo)
        {
            var frameInfo = _frameInfoService.GetFrameInfo(_channelIndex, _cameraIndex).DeepClone();
            frameInfo.FrameTag = grabInfo.FrameTag;
            frameInfo.ImageInfo = grabInfo.ImageInfo;
            return frameInfo;
        }

        public IFrameGrabber Camera
        {
            get { return _camera; }
        }

        public IObservable<FrameInfo> FrameStateChangedEvent
        {
            get { return _frameStateChangedEvent; }
        }

        public void LoadImageFile(string fileName)
        {
            var bs = fileName.GetBitmapSource();
            var ii = bs.ToImageInfo();

            var sii = GetFrameInfo(new GrabInfo(0, "ImageFile", ii));
            BufferSurfaceImageInfo = sii;

            sii.State = FrameState.Grabbing;
            _frameStateChangedEvent.OnNext(sii);
            sii.State = FrameState.Grabbed;
            _frameStateChangedEvent.OnNext(sii);
        }

        public void LoadBufferImageInfo()
        {
            var sii = GetFrameInfo(new GrabInfo(0, "Buffer", BufferSurfaceImageInfo.ImageInfo));
            BufferSurfaceImageInfo = sii;

            sii.State = FrameState.Grabbing;
            _frameStateChangedEvent.OnNext(sii);
            sii.State = FrameState.Grabbed;
            _frameStateChangedEvent.OnNext(sii);
        }

        public FrameInfo BufferSurfaceImageInfo { get; private set; }
    }
}