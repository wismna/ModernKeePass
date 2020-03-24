using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IImportFormat
    {
        Task<List<Dictionary<string, string>>> Import(string path);
    }
}