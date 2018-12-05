using System.Reactive.Subjects;

namespace Core.Reactive
{
    public interface IRetainedSubject<T> : ISubject<T>, IValueObservable<T>
    {
    }
}