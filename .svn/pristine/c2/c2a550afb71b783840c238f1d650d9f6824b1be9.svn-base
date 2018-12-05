using System;
using Microsoft.Practices.Unity;

namespace Core.Patterns
{
    public static class EventHandlerExtensions
    {
        public static IUnityContainer RegisterEventHandler<TEvent, TEventHandler>(this IUnityContainer container)
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent> 
        {
            container.RegisterType<IEventHandler<TEvent>, TEventHandler>(Guid.NewGuid().ToString());
            return container;
        }
    }
}