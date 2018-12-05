using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Core.Collections.Generic;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv.ImageAcquisition
{
    public class SinglePortSimFrameGrabberWorker
    {
        public static SinglePortSimFrameGrabberWorker Worker
        {
            get
            {
                if (_worker != null) return _worker;

                _worker = new SinglePortSimFrameGrabberWorker();
                _worker.Initialize();
                return _worker;
            }
        }

        private int _counter;
        private readonly Subject<Payload> _acquisitionCompletedEvent = new Subject<Payload>();
        private static SinglePortSimFrameGrabberWorker _worker;

        public void Initialize()
        {
        }

        public Subject<Payload> AcquisitionCompletedEvent
        {
            get { return _acquisitionCompletedEvent; }
        }

        public void Trigger(ImageInfo imageInfo)
        {
            _acquisitionCompletedEvent.OnNext(new Payload()
            {
                Msg = _counter,
                ImageInfo = imageInfo,
            });

            _counter++;
        }

    }

    public class Payload
    {
        public long Msg { get; set; }
        public ImageInfo ImageInfo { get; set; }
    }

    public class SinglePortSimFrameGrabber : IFrameGrabber
    {
        private int _index;
        private readonly Collection<string> _fileNames = new Collection<string>();
//        private ConcurrentDictionary<string, ImageInfo> _bitmapSources = new ConcurrentDictionary<string, ImageInfo>();
        private readonly Subject<GrabInfo> _grabStateChangedEvent = new Subject<GrabInfo>();

        private int inc;
        private static int counter;

        public SinglePortSimFrameGrabber()
        {
            inc = counter;
            counter++;
            Debug.WriteLine("SimObservableCamera: " + inc);
        }

        public SinglePortSimFrameGrabber(IEnumerable<string> fileNames)
        {
            _fileNames.AddRange(fileNames);
        }

        public SinglePortSimFrameGrabber(params string[] fileNames)
        {
            _fileNames.AddRange(fileNames);
        }

        public void Initialize()
        {
            SinglePortSimFrameGrabberWorker.Worker.AcquisitionCompletedEvent.Subscribe(
                 x =>
                 {
                     Tag = (int)x.Msg;
                     var frameTag = Tag;

                     if (PhaseCount > 0)
                     {
                         var mod = Tag % PhaseCount;
                         if (mod != Phase)
                             return;

                         frameTag = Tag / PhaseCount;
                     }

                     var acquisitionResult = new GrabInfo()
                     {
                         FrameTag = frameTag,
                         FrameGrabberName = Name,
                         ImageInfo = x.ImageInfo,
                         State = GrabState.Completed,
                     };
                     acquisitionResult.State = GrabState.Started;
                     _grabStateChangedEvent.OnNext(acquisitionResult);
                     //            Thread.Sleep(200);
                     acquisitionResult.State = GrabState.Completed;
                     _grabStateChangedEvent.OnNext(acquisitionResult);
                 });
        }

        public IObservable<GrabInfo> GrabStateChangedEvent
        {
            get { return _grabStateChangedEvent; }
        }

        public void Trigger()
        {
            Trigger(() => { });
        }

        public int Tag { get; private set; }

        public int Phase { get; set; }

        public int PhaseCount { get; set; }

        public void Trigger(Action beforeTriggerAction)
        {
            var currentIndex = _index;

            _index++;

            if (_index == _fileNames.Count)
                _index = 0;

            string fileName = _fileNames[currentIndex];
            var bs = fileName.GetBitmapSource();
            var imageInfo = bs.ToImageInfo();

            SinglePortSimFrameGrabberWorker.Worker.Trigger(imageInfo);
        }

        public async void Trigger(ImageInfo imageInfo)
        {
            var currentTag = Tag;
            Tag++;

            _grabStateChangedEvent.OnNext(new GrabInfo()
            {
                FrameGrabberName = Name,
                FrameTag = currentTag,
                State = GrabState.Started,
            });

            await Task.Run(() => Thread.Sleep(ProcessTime));

            _grabStateChangedEvent.OnNext(new GrabInfo(Name, imageInfo)
            {
                FrameTag = currentTag,
                State = GrabState.Completed,
            });
        }

        public void Trigger(string fileName)
        {
            var bs = fileName.GetBitmapSource();
            var imageInfo = bs.ToImageInfo();

            Trigger(imageInfo);
        }

        public Collection<string> FileNames
        {
            get { return _fileNames; }
        }

        public void Clear()
        {
            _index = 0;
            _fileNames.Clear();
        }

        public int Index { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// millisecondsTimeout
        /// </summary>
        public int ProcessTime { get; set; }
    }
}