using System.Threading.Tasks;
using Windows.Storage;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Interfaces
{
    public interface IImportService<in T> where T: IFormat
    {
        Task Import(T format, IStorageFile source, GroupVm group);
    }
}