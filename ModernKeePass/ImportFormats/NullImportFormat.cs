using Windows.Storage;
using ModernKeePass.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ModernKeePass.ImportFormats
{
    public class NullImportFormat: IFormat
    {
        public Task<List<Dictionary<string, string>>> Import(IStorageFile source)
        {
            throw new NotImplementedException();
        }
    }
}