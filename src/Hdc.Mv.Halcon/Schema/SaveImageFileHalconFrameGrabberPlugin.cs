using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using Hdc.Mv.Inspection;
using Core.Reactive.Linq;

namespace Hdc.Mv
{
    public class SaveImageFileHalconFrameGrabberPlugin : ISaveImageFileHalconFrameGrabberPlugin
    {
        private IHalconFrameInspectionController _frameInspectionController;
        private readonly Subject<HalconImageFileSavedResult> _imageFileSavedEvent = new Subject<HalconImageFileSavedResult>();

        private void SaveImage(HalconFrameInspectionInfo x)
        {
            //            Debug.WriteLine("SaveImageFileCameraCameraPlugin.SaveImage GlobalFrameTag: " + x.FrameInfo.SurfaceTag);
            if (!SaveImageEnabled)
                return;

            if (!Directory.Exists(SaveImageDirectory))
                Directory.CreateDirectory(SaveImageDirectory);

            //            var fileName = x.FrameInfo.ChannelName + "_" + x.FrameInfo.FrameGrabberName +
            //                           DateTime.Now.ToString("_yyyy-MM-dd_HH.mm.ss_fff") + ".tif";

            string ext;
            switch (Format)
            {
                case "tiff":
                    ext = ".tif";
                    break;
                case "bmp":
                    ext = ".bmp";
                    break;
                case "jpeg":
                    ext = ".jpg";
                    break;
                case "png":
                    ext = ".png";
                    break;
                case "tiff lzw":
                    ext = ".tif";
                    break;
                default:
                    throw new NotSupportedException("SaveImageFileHalconFrameGrabberPlugin.Format cannot be use: " + Format);
            }

//            var fileName = "Channel." + x.FrameInfo.ChannelIndex + "_Grabber." + x.FrameInfo.FrameGrabberIndex +
//                           DateTime.Now.ToString("_yyyy-MM-dd_HH.mm.ss_fff") + ext;
            var fileName = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss_fff") + "_Grabber." + x.FrameInfo.FrameGrabberIndex + ext;
            var fileFullName = SaveImageDirectory + @"\" + fileName;

            var image = x.CalibratedImage.Clone();
            image.WriteImage(Format, FillColor, fileFullName);
            image.Dispose();

            var fileInfo = new FileInfo(fileFullName);
            var result = new HalconImageFileSavedResult()
                         {
                             FileFullName = fileFullName,
                             FileName = fileName,
                             FileInfo = fileInfo,
                             FrameGrabberSchema = _frameInspectionController.FrameGrabberSchema,
                         };
            _imageFileSavedEvent.OnNext(result);
        }

        public IObservable<HalconImageFileSavedResult> ImageFileSavedEvent
        {
            get { return _imageFileSavedEvent; }
        }

        public IList<HalconImageFileSavedResult> GetImageFileSavedResults()
        {
            var ImageFileSavedResults = new List<HalconImageFileSavedResult>();

            if (Directory.Exists(SaveImageDirectory))
            {
                var files = Directory.GetFiles(SaveImageDirectory);
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var result = new HalconImageFileSavedResult()
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

        public string Format { get; set; }
        public int FillColor { get; set; }

        public void Initialize(IHalconFrameInspectionController frameInspectionController)
        {
            _frameInspectionController = frameInspectionController;

            frameInspectionController.FrameInspectionStateChangedEvent
//                .ObserveOnTaskPool()
                .Subscribe(x =>
                           {
                               if (x.State == FrameState.Calibrated)
                                   SaveImage(x);
                           });
        }
    }
}