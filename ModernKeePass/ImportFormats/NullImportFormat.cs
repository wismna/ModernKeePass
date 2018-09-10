using Windows.Storage;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ImportFormats
{
    public class NullImportFormat: IFormat
    {
        public IPwEntity Import(IStorageFile source)
        {
            throw new System.NotImplementedException();
        }
    }
}