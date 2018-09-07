using Windows.Storage;

namespace ModernKeePass.Interfaces
{
    public interface IImportService<in T> where T: IFormat
    {
        void Import(T format, IStorageFile source, IDatabaseService database);
    }
}