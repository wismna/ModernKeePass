using System;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Services;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DonatePage
    {
        public DonateVm Model => DataContext as DonateVm;

        public DonatePage()
        {
            InitializeComponent();
        }

        private  void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var source = sender as RadioButton;
            Model.SelectedItem = source?.DataContext as ProductListing;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var resource = new ResourcesService();
            try
            {
                var result = await Model.Purchase();
                switch ((LicenseService.PurchaseResult)result)
                {
                    case LicenseService.PurchaseResult.Succeeded:
                        MessageDialogHelper.ShowNotificationDialog(resource.GetResourceValue("DonateSucceededTitle"), resource.GetResourceValue("DonateSucceededMessage"));
                        break;
                    case LicenseService.PurchaseResult.NothingToFulfill:
                        MessageDialogHelper.ShowNotificationDialog(resource.GetResourceValue("DonateNothingToFulfillTitle"), resource.GetResourceValue("DonateNothingToFulfillMessage"));
                        break;
                    case LicenseService.PurchaseResult.PurchasePending:
                        MessageDialogHelper.ShowNotificationDialog(resource.GetResourceValue("DonatePurchasePendingTitle"), resource.GetResourceValue("DonatePurchasePendingMessage"));
                        break;
                    case LicenseService.PurchaseResult.PurchaseReverted:
                        MessageDialogHelper.ShowNotificationDialog(resource.GetResourceValue("DonatePurchaseRevertedTitle"), resource.GetResourceValue("DonatePurchaseRevertedMessage"));
                        break;
                    case LicenseService.PurchaseResult.ServerError:
                        MessageDialogHelper.ShowNotificationDialog(resource.GetResourceValue("DonateServerErrorTitle"), resource.GetResourceValue("DonateServerErrorMessage"));
                        break;
                    case LicenseService.PurchaseResult.NotPurchased:
                        MessageDialogHelper.ShowNotificationDialog(resource.GetResourceValue("DonateNotPurchasedTitle"), resource.GetResourceValue("DonateNotPurchasedMessage"));
                        break;
                    // Should never happen because these are consumables
                    case LicenseService.PurchaseResult.AlreadyPurchased:
                        MessageDialogHelper.ShowNotificationDialog(resource.GetResourceValue("DonateAlreadyPurchasedTitle"), resource.GetResourceValue("DonateAlreadyPurchasedMessage"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception)
            {
                MessageDialogHelper.ShowErrorDialog(exception);
            }
        }
    }
}
