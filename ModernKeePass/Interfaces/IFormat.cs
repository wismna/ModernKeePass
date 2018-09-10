using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace ModernKeePass.Interfaces
{
    public interface IFormat
    {
        Task<List<Dictionary<string, string>>> Import(IStorageFile source);
    }
}