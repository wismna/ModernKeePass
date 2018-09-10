using Windows.Storage;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ImportFormats
{
    public class CsvImportFormat: IFormat
    {
        public IPwEntity Import(IStorageFile source)
        {
            throw new System.NotImplementedException();
        }
    }
}