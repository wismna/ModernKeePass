using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModernKeePass.Domain.Interfaces
{
    public interface IImportFormat
    {
        Task<List<Dictionary<string, string>>> Import(string path);
    }
}