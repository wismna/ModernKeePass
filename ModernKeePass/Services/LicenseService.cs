using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Services
{
    public class LicenseService : SingletonServiceBase<LicenseService>, ILicenseService
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
            var listing = CurrentApp.LoadListingInformationAsync().GetAwaiter().GetResult();
            Products = listing.ProductListings;
        }

        public async Task<int> Purchase(string addOn)
        {
            var purchaseResults = await CurrentApp.RequestProductPurchaseAsync(addOn);
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
            var result = await CurrentApp.ReportConsumableFulfillmentAsync(productName, transactionId);
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
