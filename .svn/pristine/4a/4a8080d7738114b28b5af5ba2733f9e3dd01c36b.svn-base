using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using Core;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Production
{
    public class SimFrameInfoService : IFrameInfoService
    {
        private int _surfaceTag = -1;

        private readonly ConcurrentDictionary<int, FrameInfo> _surfaceStateInfos =
            new ConcurrentDictionary<int, FrameInfo>();

        private readonly Subject<int> _surfaceTagChangedEvent = new Subject<int>();

        public void Initialize(MvSchema schema)
        {
            Step();
        }

        public FrameInfo GetFrameInfo(int channelIndex, int frameGrabberIndex)
        {
            var index = channelIndex*FrameGrabberCount + frameGrabberIndex;
            var count = _surfaceStateInfos.Count;
            var i = count - FrameCountPerStep + index;

            var frameInfo = _surfaceStateInfos.Values.ToList()[i];

            return frameInfo.DeepClone();
        }

        public FrameInfo GetFrameInfo(int surfaceTag)
        {
            return _surfaceStateInfos[surfaceTag].DeepClone();
        }

        public void ChangeFrameState(int frameGrabberIndex, int surfaceTag, FrameState state)
        {
            _surfaceStateInfos[surfaceTag].State = state;
        }

        public void Step()
        {
            for (int i = 0; i < ChannelCount; i++)
            {
                for (int j = 0; j < FrameGrabberCount; j++)
                {
                    _surfaceTag++;

                    var channelIndex = i;
                    var frameGrabberIndex = j;

                    var workpieceTag = _surfaceTag/SurfaceCount;
                    var surfaceTag = _surfaceTag;

                    var workpieceIndex = _surfaceTag/SurfaceCount/WorkpieceCount;
                    var surfaceIndex = _surfaceTag%SurfaceCount;

                    _surfaceStateInfos.TryAdd(_surfaceTag, new FrameInfo()
                                                           {
                                                               WorkpieceTag = workpieceTag,
                                                               SurfaceTag = surfaceTag,
                                                               WorkpieceIndex = workpieceIndex,
                                                               SurfaceIndex = surfaceIndex,
                                                               ChannelIndex = channelIndex,
                                                               FrameGrabberIndex = frameGrabberIndex,
                                                               State = FrameState.Unavailable,
                                                           });
                }
            }

            _surfaceTagChangedEvent.OnNext(_surfaceTag);
        }

        public IObservable<int> SurfaceTagChangedEvent
        {
            get { return _surfaceTagChangedEvent; }
        }

        public int ChannelCount { get; set; }
        public int FrameGrabberCount { get; set; }

        public int WorkpieceCount { get; set; }
        public int SurfaceCount { get; set; }

        public int FrameCountPerStep
        {
            get { return ChannelCount*FrameGrabberCount; }
        }
    }
}