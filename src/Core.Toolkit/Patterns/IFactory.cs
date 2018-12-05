namespace Core.Patterns
{
    public interface IFactory<out T>
    {
        T Create();
    }
}