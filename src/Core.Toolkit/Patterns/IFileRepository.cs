namespace Core.Patterns
{
    public interface IFileRepository<TData> : IFileImporter<TData>, IFileExporter<TData>
    {
    }
}