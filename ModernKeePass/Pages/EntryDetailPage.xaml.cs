using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Page Détail de l'élément, consultez la page http://go.microsoft.com/fwlink/?LinkId=234232

namespace ModernKeePass.Pages
{
    /// <summary>
    /// Page affichant les détails d'un élément au sein d'un groupe, offrant la possibilité de
    /// consulter les autres éléments qui appartiennent au même groupe.
    /// </summary>
    public sealed partial class EntryDetailPage
    {
        public EntryVm Model => (EntryVm) DataContext;

        /// <summary>
        /// NavigationHelper est utilisé sur chaque page pour faciliter la navigation et 
        /// gestion de la durée de vie des processus
        /// </summary>
        public NavigationHelper NavigationHelper { get; }

        public EntryDetailPage()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Remplit la page à l'aide du contenu passé lors de la navigation. Tout état enregistré est également
        /// fourni lorsqu'une page est recréée à partir d'une session antérieure.
        /// </summary>
        /// <param name="sender">
        /// Source de l'événement ; en général <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Données d'événement qui fournissent le paramètre de navigation transmis à
        /// <see cref="Frame.Navigate(Type, object)"/> lors de la requête initiale de cette page et
        /// un dictionnaire d'état conservé par cette page durant une session
        /// antérieure.  L'état n'aura pas la valeur Null lors de la première visite de la page.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) {}

        #region Inscription de NavigationHelper

        /// Les méthodes fournies dans cette section sont utilisées simplement pour permettre
        /// NavigationHelper pour répondre aux méthodes de navigation de la page.
        /// 
        /// La logique spécifique à la page doit être placée dans les gestionnaires d'événements pour  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// et <see cref="Common.NavigationHelper.SaveState"/>.
        /// Le paramètre de navigation est disponible dans la méthode LoadState 
        /// en plus de l'état de page conservé durant une session antérieure.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
            if (!(e.Parameter is EntryVm)) return;
            DataContext = (EntryVm)e.Parameter;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var isRecycleBinEnabled = ((App)Application.Current).Database.RecycleBinEnabled && !Model.ParentGroup.IsSelected;
            var message = isRecycleBinEnabled
                ? "Are you sure you want to send this entry to the recycle bin?"
                : "Are you sure you want to delete this entry?";
            var text = isRecycleBinEnabled ? "Item moved to the Recycle bin" : "Item permanently removed";
            MessageDialogHelper.ShowDeleteConfirmationDialog("Delete", message, a =>
            {
                ToastNotificationHelper.ShowMovedToast(Model, "Deleting", text);
                Model.MarkForDelete();
                if (Frame.CanGoBack) Frame.GoBack();
            });
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            ToastNotificationHelper.ShowMovedToast(Model, "Restored", "Item returned to its original group");
            if (Frame.CanGoBack) Frame.GoBack();
        }

        private async void UrlButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uri = new Uri(UrlTextBox.Text);
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }
            catch
            {
                // TODO: Show some error
            }
        }

    }
}
