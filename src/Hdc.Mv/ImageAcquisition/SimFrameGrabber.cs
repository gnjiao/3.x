using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Core.Collections.Generic;
using Hdc.Mv.Production;
using Core.Windows.Media.Imaging;
using Core.Windows.Threading;

namespace Hdc.Mv.ImageAcquisition
{
    [Serializable]
    public class SimFrameGrabber : IFrameGrabber
    {
        private int _index;
        private readonly Collection<string> _fileNames = new Collection<string>();
//        private ConcurrentDictionary<string, ImageInfo> _bitmapSources = new ConcurrentDictionary<string, ImageInfo>();
        private readonly Subject<GrabInfo> _grabStateChangedEvent = new Subject<GrabInfo>();

        private int inc;
        private static int counter;

        public SimFrameGrabber()
        {
            inc = counter;
            counter++;
            Debug.WriteLine("SimObservableCamera: " + inc);
        }

        public SimFrameGrabber(IEnumerable<string> fileNames)
        {
            _fileNames.AddRange(fileNames);
        }

        public SimFrameGrabber(params string[] fileNames)
        {
            _fileNames.AddRange(fileNames);
        }

        public void Initialize()
        {
        }

        public IObservable<GrabInfo> GrabStateChangedEvent
        {
            get { return _grabStateChangedEvent; }
        }

        public void Trigger()
        {
            Trigger(new Action(()=>{}));
        }

        public int Tag { get; private set; }

        public async void Trigger(Action beforeTriggerAction)
        {
            Debug.WriteLine("SimFrameGrabber.Trigger begin, at " + DateTime.Now);
            var currentTag = Tag;
            var currentIndex = _index;

            Tag++;
            _index++;

            if (_index == _fileNames.Count)
                _index = 0;

            Debug.WriteLine("SimFrameGrabber.Trigger.OnNext(GrabState.Started) begin, at " + DateTime.Now);
            _grabStateChangedEvent.OnNext(new GrabInfo()
                                          {
                                              FrameGrabberName = Name,
                                              FrameTag = currentTag,
                                              State = GrabState.Started
                                          });
            Debug.WriteLine("SimFrameGrabber.Trigger.OnNext(GrabState.Started) end, at " + DateTime.Now);

//            return;

            ImageInfo ii = await Task.Run(() =>
                                          {
                                              Debug.WriteLine("SimFrameGrabber.Trigger.Sleep begin, at " + DateTime.Now);

//                                              await Task.Delay(ProcessTime);
                                              Thread.Sleep(ProcessTime);

                                              string fileName = _fileNames[currentIndex];

                                              var bs = fileName.GetBitmapSource();
                                              var imageInfo = bs.ToImageInfo();
                                              Debug.WriteLine("SimFrameGrabber.Trigger.Sleep end, at " + DateTime.Now);
                                              return imageInfo;
                                          });

            Debug.WriteLine("SimFrameGrabber.Trigger.OnNext(GrabState.Completed) begin, at " + DateTime.Now);
            _grabStateChangedEvent.OnNext(new GrabInfo(Name, ii)
                                          {
                                              FrameTag = currentTag,
                                              State = GrabState.Completed,
                                          });
            Debug.WriteLine("SimFrameGrabber.Trigger.OnNext(GrabState.Completed) end, at " + DateTime.Now);
            Debug.WriteLine("SimFrameGrabber.Trigger end, at " + DateTime.Now);
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