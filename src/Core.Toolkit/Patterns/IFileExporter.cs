namespace Core.Patterns
{
    public interface IFileExporter<in TData>
    {
        void Export(TData data, string fileName);
    }
}