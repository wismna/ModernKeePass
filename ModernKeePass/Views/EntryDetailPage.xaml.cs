using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.Models;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Page Détail de l'élément, consultez la page http://go.microsoft.com/fwlink/?LinkId=234232

namespace ModernKeePass.Views
{
    /// <summary>
    /// Page affichant les détails d'un élément au sein d'un groupe, offrant la possibilité de
    /// consulter les autres éléments qui appartiennent au même groupe.
    /// </summary>
    public sealed partial class EntryDetailPage
    {
        public EntryDetailVm Model => (EntryDetailVm) DataContext;

        /// <summary>
        /// NavigationHelper est utilisé sur chaque page pour faciliter la navigation et 
        /// gestion de la durée de vie des processus
        /// </summary>
        public NavigationHelper NavigationHelper { get; }
        
        public EntryDetailPage()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
        }
        
        #region Inscription de NavigationHelper

        /// Les méthodes fournies dans cette section sont utilisées simplement pour permettre
        /// NavigationHelper pour répondre aux méthodes de navigation de la page.
        /// 
        /// La logique spécifique à la page doit être placée dans les gestionnaires d'événements pour  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// et <see cref="Common.NavigationHelper.SaveState"/>.
        /// Le paramètre de navigation est disponible dans la méthode LoadState 
        /// en plus de l'état de page conservé durant une session antérieure.

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
            var args = e.Parameter as NavigationItem;
            if (args != null)
            {
                DataContext = new EntryDetailVm(args.Id) { IsEditMode = args.IsNew };
                if (args.IsNew) await Model.GeneratePassword();
            }
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            await Model.AddHistory();
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        
        private void EntryDetailPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, e.NewSize.Width < 700 ? "Small" : "Large", true);
            VisualStateManager.GoToState(TopMenu, e.NewSize.Width < 800 ? "Collapsed" : "Overflowed", true);
        }

        private async void TopMenu_OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var resource = new ResourceHelper();
            if (Model.IsCurrentEntry)
            {
                var isRecycleOnDelete = Model.IsRecycleOnDelete;

                var message = isRecycleOnDelete
                    ? resource.GetResourceValue("EntryRecyclingConfirmation")
                    : resource.GetResourceValue("EntryDeletingConfirmation");
                await MessageDialogHelper.ShowActionDialog(resource.GetResourceValue("EntityDeleteTitle"), message,
                    resource.GetResourceValue("EntityDeleteActionButton"),
                    resource.GetResourceValue("EntityDeleteCancelButton"), async a =>
                    {
                        var text = isRecycleOnDelete ? resource.GetResourceValue("EntryRecycled") : resource.GetResourceValue("EntryDeleted");
                        //ToastNotificationHelper.ShowMovedToast(Entity, _resource.GetResourceValue("EntityDeleting"), text);
                        await Model.MarkForDelete(resource.GetResourceValue("RecycleBinTitle"));
                        NavigationHelper.GoBack();
                    }, null);
            }
            else
            {
                await MessageDialogHelper.ShowActionDialog(resource.GetResourceValue("HistoryDeleteTitle"), resource.GetResourceValue("HistoryDeleteDescription"),
                    resource.GetResourceValue("EntityDeleteActionButton"),
                    resource.GetResourceValue("EntityDeleteCancelButton"), async a =>
                    {
                        //ToastNotificationHelper.ShowMovedToast(Entity, _resource.GetResourceValue("EntityDeleting"), text);
                        await Model.DeleteHistory();
                    }, null);
            }
        }
    }
}
