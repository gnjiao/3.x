using System.ComponentModel;

namespace Core.ComponentModel
{
    public interface INotifyPropertyWrapper<T> : INotifyPropertyChanged
    {
        T Object { get; set; }
    }
}