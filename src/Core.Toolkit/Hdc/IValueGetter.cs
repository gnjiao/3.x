namespace Core
{
    public interface IValueGetter<out T>
    {
        T Value { get; }
    }
}