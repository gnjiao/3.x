using System;
using Core.Patterns;

namespace Core.Patterns
{
    public interface IEventAggregator
    {
        IObservable<TEvent> GetEvents<TEvent>() where TEvent : IEvent;
    }
}