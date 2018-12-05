namespace Core
{
    public interface IObjectProvider<out TObject>
    {
        TObject Object { get; }
    }
}