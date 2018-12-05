using System;
using System.Reactive.Linq;

namespace Core.Reactive
{
    public class BooleanToInt32ValueObservableWrapper : IValueObservable<int>
    {
        private readonly IValueObservable<bool> _valueObservable;

        public BooleanToInt32ValueObservableWrapper(IValueObservable<bool> valueObservable)
        {
            _valueObservable = valueObservable;
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            return _valueObservable.Select(x =>
            {
                var int32 = Convert.ToInt32(x);
                return int32;
            }).Subscribe(observer);
        }

        int IValueGetter<int>.Value
        {
            get { return Convert.ToInt32(_valueObservable.Value); }
        }

        int IValueObservable<int>.Value
        {
            get { return Convert.ToInt32(_valueObservable.Value); }
            set { _valueObservable.Value = Convert.ToBoolean(value); }
        }
    }
}