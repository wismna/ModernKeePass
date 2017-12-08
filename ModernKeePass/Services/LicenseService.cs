using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Services
{
    public class LicenseService : ILicenseService
    {
        public enum PurchaseResult
        {
            Succeeded,
            NothingToFulfill,
            PurchasePending,
            PurchaseReverted,
            ServerError,
            NotPurchased,
            AlreadyPurchased
        }

        public IReadOnlyDictionary<string, ProductListing> Products { get; }

        //private LicenseInformation _licenseInformation;
        private readonly HashSet<Guid> _consumedTransactionIds = new HashSet<Guid>();

        public LicenseService()
        {
            // Initialize the license info for use in the app that is uploaded to the Store.
            // Uncomment the following line in the release version of your app.
            //_licenseInformation = CurrentApp.LicenseInformation;

            // Initialize the license info for testing.
            // Comment the following line in the release version of your app.
            //_licenseInformation = CurrentAppSimulator.LicenseInformation;
#if DEBUG
            try
            {
                var proxyFile = Package.Current.InstalledLocation.GetFileAsync("data\\WindowsStoreProxy.xml").GetAwaiter().GetResult();
                CurrentAppSimulator.ReloadSimulatorAsync(proxyFile).GetAwaiter().GetResult();
            }
            catch { }
            var listing = CurrentAppSimulator.LoadListingInformationAsync().GetAwaiter().GetResult();
#else
            var listing = CurrentApp.LoadListingInformationAsync().GetAwaiter().GetResult();
#endif
            Products = listing.ProductListings;
        }

        public async Task<int> Purchase(string addOn)
        {
#if DEBUG
            var purchaseResults = await CurrentAppSimulator.RequestProductPurchaseAsync(addOn);
#else
            var purchaseResults = await CurrentApp.RequestProductPurchaseAsync(addOn);
#endif
            switch (purchaseResults.Status)
            {
                case ProductPurchaseStatus.Succeeded:
                    GrantFeatureLocally(purchaseResults.TransactionId);
                    return (int) await ReportFulfillmentAsync(purchaseResults.TransactionId, addOn);
                case ProductPurchaseStatus.NotFulfilled:
                    // The purchase failed because we haven't confirmed fulfillment of a previous purchase.
                    // Fulfill it now.
                    if (!IsLocallyFulfilled(purchaseResults.TransactionId))
                    {
                        GrantFeatureLocally(purchaseResults.TransactionId);
                    }
                    return (int) await ReportFulfillmentAsync(purchaseResults.TransactionId, addOn);
                case ProductPurchaseStatus.NotPurchased:
                    return (int) PurchaseResult.NotPurchased;
                case ProductPurchaseStatus.AlreadyPurchased:
                    return (int) PurchaseResult.AlreadyPurchased;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<PurchaseResult> ReportFulfillmentAsync(Guid transactionId, string productName)
        {
#if DEBUG
            var result = await CurrentAppSimulator.ReportConsumableFulfillmentAsync(productName, transactionId);
#else
            var result = await CurrentApp.ReportConsumableFulfillmentAsync(productName, transactionId);
#endif
            return (PurchaseResult) result;
        }

        private void GrantFeatureLocally(Guid transactionId)
        {
            _consumedTransactionIds.Add(transactionId);
        }

        private bool IsLocallyFulfilled(Guid transactionId)
        {
            return _consumedTransactionIds.Contains(transactionId);
        }
    }
}
