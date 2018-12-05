using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive.Subjects;
using Hdc.Mv.Production;
using Core.Reactive.Linq;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv.Inspection
{
    public class SaveImageFilePlugin : ISaveImageFilePlugin
    {
        private IFrameInspectionController _frameInspectionController;
        private readonly Subject<ImageFileSavedResult> _imageFileSavedEvent = new Subject<ImageFileSavedResult>();

        public void Initialize(IFrameInspectionController frameInspectionController)
        {
            _frameInspectionController = frameInspectionController;

            frameInspectionController.FrameInspectionStateChangedEvent
                .ObserveOnTaskPool()
                .Subscribe(x =>
                {
                    if (x.State == FrameState.Grabbed)
                        SaveImage(x);
                });
        }

        private void SaveImage(FrameDefectInspectionInfo x)
        {
//            Debug.WriteLine("SaveImageFileCameraCameraPlugin.SaveImage GlobalFrameTag: " + x.FrameInfo.SurfaceTag);
            if (!SaveImageEnabled)
                return;

            if (!Directory.Exists(SaveImageDirectory))
                Directory.CreateDirectory(SaveImageDirectory);

//            var fileName = x.FrameInfo.ChannelName + "_" + x.FrameInfo.FrameGrabberName +
//                           DateTime.Now.ToString("_yyyy-MM-dd_HH.mm.ss_fff") + ".tif";

            var fileName = "Channel." +  x.FrameInfo.ChannelIndex + "_Grabber." + x.FrameInfo.FrameGrabberIndex +
                           DateTime.Now.ToString("_yyyy-MM-dd_HH.mm.ss_fff") + ".tif";
            var fileFullName = SaveImageDirectory + @"\" + fileName;

            var bs = x.FrameInfo.ImageInfo.ToBitmapSource();
            bs.SaveToTiff(fileFullName);

            var fileInfo = new FileInfo(fileFullName);
            var result = new ImageFileSavedResult()
                         {
                             FileFullName = fileFullName,
                             FileName = fileName,
                             FileInfo = fileInfo,
                             FrameGrabberSchema = _frameInspectionController.FrameGrabberSchema,
                         };
            _imageFileSavedEvent.OnNext(result);
        }

        public IObservable<ImageFileSavedResult> ImageFileSavedEvent
        {
            get { return _imageFileSavedEvent; }
        }

        public IList<ImageFileSavedResult> GetImageFileSavedResults()
        {
            var ImageFileSavedResults = new List<ImageFileSavedResult>();

            if (Directory.Exists(SaveImageDirectory))
            {
                var files = Directory.GetFiles(SaveImageDirectory);
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var result = new ImageFileSavedResult()
                                 {
                                     FileFullName = fileInfo.FullName,
                                     FileName = fileInfo.Name,
                                     FileInfo = fileInfo,
                                     FrameGrabberSchema = _frameInspectionController.FrameGrabberSchema,
                                 };
                    ImageFileSavedResults.Add(result);
                }
            }

            return ImageFileSavedResults;
        }

        public bool SaveImageEnabled { get; set; }
        public string SaveImageDirectory { get; set; }
    }
}