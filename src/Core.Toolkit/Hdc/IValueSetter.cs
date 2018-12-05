namespace Core
{
    public interface IValueSetter<in T>
    {
        T Value { set; }
    }
}