using System.Collections.Generic;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IImportFormat
    {
        List<Dictionary<string, string>> Import(IList<string> fileContents);
    }
}