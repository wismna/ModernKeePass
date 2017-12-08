using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace ModernKeePass.Interfaces
{
    public interface ILicenseService
    {
        IReadOnlyDictionary<string, ProductListing> Products { get; }
        Task<int> Purchase(string addOn);
    }
}
