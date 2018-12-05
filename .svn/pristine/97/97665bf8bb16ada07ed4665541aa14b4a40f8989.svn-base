using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Core.Collections.Generic;
using Hdc.Collections.Generic;
using Core.Collections.ObjectModel;
using Core.ComponentModel;
using Core.Diagnostics;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Inspection;
using Hdc.Mvvm;
using Core.Reactive.Linq;
using Core.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;

namespace Hdc.Mv.Mvvm
{
    public class SurfaceViewModel : ViewModel
    {
        private int _index;
        private readonly IFrameInspectionController _frameInspectionController;
        private BitmapSource _bitmapSource;
        private BitmapSource _previewBitmapSource;
        private readonly ObservableCollection<string> _imageFileNames = new ObservableCollection<string>();

        private FrameDefectInspectionInfo _frameDefectInspectionInfo;

        public BitmapSource BitmapSource
        {
            get { return _bitmapSource; }
            set
            {
                if (Equals(_bitmapSource, value)) return;
                _bitmapSource = value;
                RaisePropertyChanged(() => BitmapSource);
            }
        }

        public BitmapSource PreviewBitmapSource
        {
            get { return _previewBitmapSource; }
            set
            {
                if (Equals(_previewBitmapSource, value)) return;
                _previewBitmapSource = value;
                RaisePropertyChanged(() => PreviewBitmapSource);
            }
        }

        public BindableCollection<DefectInfo> DefectInfos { get; set; }
        public ICollectionView ImageFileNamesCollectionView { get; set; }

        public IFrameInspectionController FrameInspectionController
        {
            get { return _frameInspectionController; }
        }

        private FrameState _frameState;

        public FrameState FrameState
        {
            get { return _frameState; }
            set
            {
                if (Equals(_frameState, value)) return;
                _frameState = value;
                RaisePropertyChanged(() => FrameState);
            }
        }

        private static int instanceCounter;

        public SurfaceViewModel(IFrameInspectionController frameInspectionController)
        {
            _index = instanceCounter;
            instanceCounter++;

            _frameInspectionController = frameInspectionController;
            DefectInfos = new BindableCollection<DefectInfo>();

            _frameInspectionController.FrameInspectionStateChangedEvent
                .ObserveOnTaskPool()
//                .ObserveOnDispatcher()
                .Subscribe(x =>
                {
//                    var now = DateTime.Now;
//                    Debug.WriteLine("FrameInspectionStateChangedEvent[{0}].State: {1}, at {2}", _index,
//                        x.FrameInfo.State, now);

                    _frameDefectInspectionInfo = x;

                    if (x.FrameInfo.State != FrameState.Inspected)
                        FrameState = x.FrameInfo.State;
                    else
                        FrameState = x.DefectInfos.Any()
                            ? FrameState.InspectedWithRejected
                            : FrameState.InspectedWithAccepted;

                    switch (x.FrameInfo.State)
                    {
                        case FrameState.Grabbing:
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                                new Action(() =>
                                {
                                    BitmapSource = null;
                                    DefectInfos.Clear();
                                }));

                            OnGrabbing();
                            break;
                        case FrameState.Grabbed:
//                            break;
//                                var sw = new NotifyStopwatch("FrameInspectionStateChangedEvent ToBitmapSource: " +
//                                                             x.FrameInfo.FrameTag);
                            var bs = x.FrameInfo.ImageInfo.ToBitmapSource();
//                                sw.Dispose();
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                                new Action(() =>
                                {
                                    var wbs2 = x.FrameInfo.ImageInfo.ToBitmapSource();

                                    BitmapSource = wbs2;
                                }));

//                                var sw2 =
//                                    new NotifyStopwatch(
//                                        "FrameInspectionStateChangedEvent ConvertToPbgra32Format: " +
//                                        x.FrameInfo.FrameTag);
                            int width = bs.PixelWidth/8;
                            int height = bs.PixelHeight/8;
                            var wbs = BitmapFactory.ConvertToPbgra32Format(bs);
//                                sw2.Dispose();

//                                var sw3 =
//                                          new NotifyStopwatch("FrameInspectionStateChangedEvent Resize: " +
//                                                              x.FrameInfo.FrameTag);
                            WriteableBitmap previewBitmapSource = wbs.Resize(width, height,
                                WriteableBitmapExtensions.Interpolation.NearestNeighbor);
//                                sw3.Dispose();

                            var ii2 = previewBitmapSource.ToImageInfo();

                            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                                new Action(() =>
                                {
                                    var wbs2 = ii2.ToBitmapSource();

                                    PreviewBitmapSource = wbs2;
                                }));

                            OnGrabbed();
                            break;
                        case FrameState.Inspecting:
                            OnInspecting();
                            break;
                        case FrameState.Inspected:
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                                new Action(() =>
                                {
                                    DefectInfos.Clear();

                                    //                            var sw4 =
                                    //                                new NotifyStopwatch("FrameInspectionStateChangedEvent DefectInfos.AddRange: " +
                                    //                                                    x.FrameInfo.FrameTag);
                                    var defectInfos = x.DefectInfos;
                                    DefectInfos.AddRange(defectInfos);
                                    //                            sw4.Dispose();
                                }));


                            OnInspected();

                            break;
                    }
                });

            _frameInspectionController.FrameGrabberSchema.SaveImageFilePlugin.ImageFileSavedEvent
                .ObserveOnDispatcher()
                .Subscribe(x => { _imageFileNames.Add(x.FileName); });

            var files =
                frameInspectionController.FrameGrabberSchema.SaveImageFilePlugin.GetImageFileSavedResults()
                    .Select(x => x.FileName);
            _imageFileNames.AddRange(files);
            ImageFileNamesCollectionView = _imageFileNames.GetDefaultCollectionView();
            ImageFileNamesCollectionView.SortDescriptions.Add(new SortDescription(".", ListSortDirection.Descending));


            SelectFileNameCommand = new DelegateCommand<string>(
                fn =>
                {
                    if (fn == null)
                        return;

                    BitmapSource = null;
                    DefectInfos.Clear();

                    FrameInspectionController.LoadImageFile(fn);
                });
        }

        protected virtual void OnGrabbing()
        {
        }


        protected virtual void OnGrabbed()
        {
        }


        protected virtual void OnInspecting()
        {
        }

        protected virtual void OnInspected()
        {
        }

        public DelegateCommand<DefectInfo> SelectDefectInfoCommand { get; set; }
        public DelegateCommand<string> SelectFileNameCommand { get; set; }

        public FrameDefectInspectionInfo FrameDefectInspectionInfo
        {
            get { return _frameDefectInspectionInfo; }
        }
    }
}