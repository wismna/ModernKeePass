using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.ViewModels;
using ModernKeePassLib.Cryptography.PasswordGenerator;

// Pour en savoir plus sur le modèle d'élément Page Détail de l'élément, consultez la page http://go.microsoft.com/fwlink/?LinkId=234232

namespace ModernKeePass.Pages
{
    /// <summary>
    /// Page affichant les détails d'un élément au sein d'un groupe, offrant la possibilité de
    /// consulter les autres éléments qui appartiennent au même groupe.
    /// </summary>
    public sealed partial class EntryDetailPage
    {
        private NavigationHelper navigationHelper;

        public EntryVm Model => (EntryVm) DataContext;

        /// <summary>
        /// NavigationHelper est utilisé sur chaque page pour faciliter la navigation et 
        /// gestion de la durée de vie des processus
        /// </summary>
        public NavigationHelper NavigationHelper => navigationHelper;

        public EntryDetailPage()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
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
            navigationHelper.OnNavigatedTo(e);
            if (!(e.Parameter is EntryVm)) return;
            DataContext = (EntryVm)e.Parameter;
            if (Model.IsEditMode)
                Task.Factory.StartNew(
                    () => Dispatcher.RunAsync(CoreDispatcherPriority.Low,
                        () => TitleTextBox.Focus(FocusState.Programmatic)));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Are you sure you want to delete this entry?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("Delete", delete =>
            {
                ToastNotificationHelper.ShowUndoToast("Entry", Model);
                Model.MarkForDelete();
                if (Frame.CanGoBack) Frame.GoBack();
            }));
            messageDialog.Commands.Add(new UICommand("Cancel"));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
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

        private void PasswordGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            Model.GeneratePassword();
            /*var button = (Button)sender;
            button?.Flyout?.Hide();*/
        }
    }
}
