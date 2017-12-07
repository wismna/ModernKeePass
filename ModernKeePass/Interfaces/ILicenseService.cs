using System.Collections.Generic;
using Windows.ApplicationModel.Store;

namespace ModernKeePass.Interfaces
{
    public interface ILicenseService
    {
        IReadOnlyDictionary<string, ProductListing> Products { get; }
    }
}
