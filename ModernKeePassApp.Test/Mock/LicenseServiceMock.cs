using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using ModernKeePass.Interfaces;

namespace ModernKeePassApp.Test.Mock
{
    public class LicenseServiceMock: ILicenseService
    {
        public IReadOnlyDictionary<string, ProductListing> Products { get; }

        public LicenseServiceMock()
        {
            try
            {
                var proxyFile = Package.Current.InstalledLocation.GetFileAsync("data\\WindowsStoreProxy.xml").GetAwaiter().GetResult();
                CurrentAppSimulator.ReloadSimulatorAsync(proxyFile).GetAwaiter().GetResult();
            }
            catch { }
            var listing = CurrentAppSimulator.LoadListingInformationAsync().GetAwaiter().GetResult();
            Products = listing.ProductListings;
        }

        public Task<int> Purchase(string addOn)
        {
            throw new System.NotImplementedException();
        }
    }
}
