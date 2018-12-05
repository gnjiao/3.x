using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Core;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Calibration;
using Hdc.Mv.Halcon;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Inspection;
using Hdc.Mv.Production;

namespace Hdc.Mv
{
    public class HalconFrameInspectionController : IHalconFrameInspectionController, IDisposable
    {
        private readonly HalconFrameGrabberSchema _frameGrabberSchema;

        private readonly ConcurrentDictionary<long, HalconFrameInspectionInfo> _frameInfos =
            new ConcurrentDictionary<long, HalconFrameInspectionInfo>();

        private readonly Subject<IList<HalconFrameInspectionInfo>> _frameInfosChangedEvent =
            new Subject<IList<HalconFrameInspectionInfo>>();

        private readonly IFrameInfoController _frameInfoController;

        private readonly Subject<HalconFrameInspectionInfo> _frameInspectionStateChangedEvent =
            new Subject<HalconFrameInspectionInfo>();

//        private readonly HalconInspectionSchemaInspector _inspector;
        private readonly IHalconInspector _inspector;
        private Thread _newWindowThread;
        private static Dispatcher WorkDispatcher { get; set; }

        public HalconFrameInspectionController(HalconFrameGrabberSchema frameGrabberSchema)
        {
            Application.Current.Exit += Current_Exit;
            Application.Current.MainWindow.Closing += MainWindow_Closing;

            _frameGrabberSchema = frameGrabberSchema;
            _inspector = (IHalconInspector)frameGrabberSchema.Inspector;

            var cameraIndex = frameGrabberSchema.Index;

            _frameInfoController = new FrameInfoController(0,
                cameraIndex,
                frameGrabberSchema.MvSchema.FrameInfoService,
                frameGrabberSchema.FrameGrabber);

            _newWindowThread = new Thread(new ThreadStart(() =>
            {
                Console.WriteLine("ThreadStart begin");
                WorkDispatcher = Dispatcher.CurrentDispatcher;
                System.Windows.Threading.Dispatcher.Run();
                Console.WriteLine("ThreadStart end");
            }));
            _newWindowThread.Start();


            _frameInfoController.FrameStateChangedEvent
                //                .ObserveOnTaskPool()
                .Subscribe(async
                    x =>
                    {
                        //                              Debug.WriteLine("CameraController 00::GlobalFrameTag: {0}, CameraFrameTag: {1}, State: {2}",
                        //                                  xx.FrameInfo.GlobalFrameTag,
                        //                                  xx.FrameInfo.CameraFrameTag,
                        //                                  xx.FrameInfo.State);
                        var fi = x.DeepClone();
                        switch (fi.State)
                        {
                            case FrameState.Grabbing:
                                Debug.WriteLine("HalconFrameInspectionController.Grabbing begin, at " + DateTime.Now);
                                _frameInfos.TryAdd(x.FrameTag, new HalconFrameInspectionInfo(fi)
                                {
                                    CalibrationDisabled = fi.ChannelIndex == 99
                                });
                                var ssi2 = _frameInfos[fi.FrameTag];
                                ssi2.State = FrameState.Grabbing;
                                var value = new HalconFrameInspectionInfo(ssi2.FrameInfo.DeepClone());
                                Debug.WriteLine("HalconFrameInspectionController.Grabbing.OnNext(Grabbing) begin, at " +
                                                DateTime.Now);
                                _frameInspectionStateChangedEvent.OnNext(value);
                                Debug.WriteLine("HalconFrameInspectionController.Grabbing.OnNext(Grabbing) end, at " +
                                                DateTime.Now);
                                Refresh();
                                Debug.WriteLine("HalconFrameInspectionController.Grabbing end, at " + DateTime.Now);
                                break;
                            case FrameState.Grabbed:
                                Debug.WriteLine("HalconFrameInspectionController.Grabbed begin, at " + DateTime.Now);
                                var ssi = _frameInfos[fi.FrameTag];
                                ssi.FrameInfo.ImageInfo = fi.ImageInfo;
                                ssi.State = FrameState.Grabbed;
                                var inspectionInfo = new HalconFrameInspectionInfo(ssi.FrameInfo.DeepClone());
                                Debug.WriteLine("HalconFrameInspectionController.Grabbed.OnNext(Grabbed) begin, at " +
                                                DateTime.Now);
                                _frameInspectionStateChangedEvent.OnNext(inspectionInfo);
                                Debug.WriteLine("HalconFrameInspectionController.Grabbed.OnNext(Grabbed) end, at " +
                                                DateTime.Now);
                                Refresh();

                                // Calibration
                                ssi.State = FrameState.Calibrating;
                                var frameInspectionInfo = new HalconFrameInspectionInfo(ssi.FrameInfo.DeepClone());
                                Debug.WriteLine(
                                    "HalconFrameInspectionController.Grabbed.OnNext(Calibrating) begin, at " +
                                    DateTime.Now);
                                _frameInspectionStateChangedEvent.OnNext(frameInspectionInfo);
                                Debug.WriteLine("HalconFrameInspectionController.Grabbed.OnNext(Calibrating) end, at " +
                                                DateTime.Now);
                                Refresh();


                                HalconFrameInspectionInfo halconFrameInspectionInfo;
                                HImage calibedImage;

                                if (!ssi.CalibrationDisabled)
                                {
//                                        calibedImage =
//                                            await
//                                                _frameGrabberSchema.Calibrator.Calibrate8BppAsync(
//                                                    ssi.FrameInfo.ImageInfo);
//                                    calibedImage = _frameGrabberSchema.Calibrator.Calibrate8Bpp(ssi.FrameInfo.ImageInfo);
//                                        calibedImage.Dispose();
//                                    return;

                                    calibedImage = WorkDispatcher.Invoke(() =>
                                    {
                                        HImage calibrate8Bpp =
                                            _frameGrabberSchema.Calibrator.Calibrate8Bpp(ssi.FrameInfo.ImageInfo);
                                        return calibrate8Bpp;
                                    });
                                }
                                else
                                {
                                    calibedImage = ssi.FrameInfo.ImageInfo.To8BppHImage();
                                    ssi.FrameInfo.ImageInfo.FreeMemory();
                                }

                                ssi.State = FrameState.Calibrated;
                                halconFrameInspectionInfo = new HalconFrameInspectionInfo(ssi.FrameInfo.DeepClone())
                                {
                                    CalibratedImage = calibedImage,
                                };

                                Debug.WriteLine(
                                    "HalconFrameInspectionController.Grabbed.OnNext(Calibrated) begin, at " +
                                    DateTime.Now);
                                _frameInspectionStateChangedEvent.OnNext(halconFrameInspectionInfo);
                                Debug.WriteLine("HalconFrameInspectionController.Grabbed.OnNext(Calibrated) end, at " +
                                                DateTime.Now);
                                Refresh();


                                // Inspection
                                ssi.State = FrameState.Inspecting;
                                var info = new HalconFrameInspectionInfo(ssi.FrameInfo.DeepClone());
                                Debug.WriteLine(
                                    "HalconFrameInspectionController.Grabbed.OnNext(Inspecting) begin, at " +
                                    DateTime.Now);
                                _frameInspectionStateChangedEvent.OnNext(info);
                                Debug.WriteLine("HalconFrameInspectionController.Grabbed.OnNext(Inspecting) end, at " +
                                                DateTime.Now);
                                Refresh();

                                Debug.WriteLine(
                                    "FrameInspectionController::_defectInspector.InspectAsync begin: " +
                                    "SurfaceTag={0}, " +
                                    "ChannelIndex={1}, " +
                                    "FrameTag={2}, " +
                                    "FrameGrabberIndex={3}, " +
                                    "State={4}, " +
                                    "DateTime={5}",
                                    ssi.FrameInfo.SurfaceTag,
                                    ssi.FrameInfo.ChannelIndex,
                                    ssi.FrameInfo.FrameTag,
                                    ssi.FrameInfo.FrameGrabberIndex,
                                    ssi.State,
                                    DateTime.Now);

                                var inspectionResult = await _inspector.InspectAsync(calibedImage);

                                Debug.WriteLine(
                                    "FrameInspectionController::_defectInspector.InspectAsync end: " +
                                    "SurfaceTag={0}, " +
                                    "ChannelIndex={1}, " +
                                    "FrameTag={2}, " +
                                    "FrameGrabberIndex={3}, " +
                                    "State={4}, " +
                                    "DefectCount={5}" +
                                    "DateTime={6}",
                                    ssi.FrameInfo.SurfaceTag,
                                    ssi.FrameInfo.ChannelIndex,
                                    ssi.FrameInfo.FrameTag,
                                    ssi.FrameInfo.FrameGrabberIndex,
                                    ssi.State,
                                    inspectionResult.RegionDefectResults.SelectMany(p => p.DefectResults).Count(),
                                    DateTime.Now);

                                //                                if (_inspector.MaxDefectCount > 0)
                                //                                {
                                //                                    if (halconFrameInspectionInfo.Count > _defectInspector.MaxDefectCount)
                                //                                    {
                                //                                        halconFrameInspectionInfo = halconFrameInspectionInfo.Take(_defectInspector.MaxDefectCount).ToList();
                                //                                    }
                                //                                }

                                ssi.State = FrameState.Inspected;
                                ssi.InspectionResult = inspectionResult;

                                var halconFrameInspectionInfo1 = new HalconFrameInspectionInfo(ssi.FrameInfo.DeepClone())
                                {
                                    CalibratedImage = calibedImage,
                                    InspectionResult = inspectionResult,
                                };
                                Debug.WriteLine("HalconFrameInspectionController.Grabbed.OnNext(Inspected) begin, at " +
                                                DateTime.Now);
                                _frameInspectionStateChangedEvent.OnNext(halconFrameInspectionInfo1);
                                Debug.WriteLine("HalconFrameInspectionController.Grabbed.OnNext(Inspected) end, at " +
                                                DateTime.Now);

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

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
//            WorkDispatcher.ExitAllFrames();
            WorkDispatcher.Thread.Abort();
            _newWindowThread.Abort();
            Application.Current.Shutdown(0);
            Environment.Exit(0);
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
//            WorkDispatcher.ExitAllFrames();
            WorkDispatcher.Thread.Abort();
            _newWindowThread.Abort();
            Application.Current.Shutdown(0);
            Environment.Exit(0);
        }

        public IFrameGrabber FrameGrabber
        {
            get { return _frameGrabberSchema.FrameGrabber; }
        }

        public HalconFrameGrabberSchema FrameGrabberSchema
        {
            get { return _frameGrabberSchema; }
        }

        public IObservable<HalconFrameInspectionInfo> FrameInspectionStateChangedEvent
        {
            get { return _frameInspectionStateChangedEvent; }
        }

        public void LoadBufferImageInfo()
        {
            _frameInfoController.LoadBufferImageInfo();
        }

        public void LoadImageFile(string fileName)
        {
            _frameInfoController.LoadImageFile(fileName);
        }

        public IFrameInfoController FrameInfoController
        {
            get { return _frameInfoController; }
        }

        public IObservable<IList<HalconFrameInspectionInfo>> FrameInfosChangedEvent
        {
            get { return _frameInfosChangedEvent; }
        }

        private void Refresh()
        {
            _frameInfosChangedEvent.OnNext(_frameInfos.Values.ToList());
        }

        public void Dispose()
        {
            Dispatcher.ExitAllFrames();
        }
    }
}