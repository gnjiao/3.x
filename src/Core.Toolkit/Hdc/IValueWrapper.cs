namespace Core
{
    public interface IValueWrapper<T>: IValueGetter<T>, IValueSetter<T>
    {
        new T Value { get; set; }
    }
}