using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
            var args = e.Parameter as string;
            if (args != null) DataContext = new EntryDetailVm(args);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        
        private void EntryDetailPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, e.NewSize.Width < 700 ? "Small" : "Large", true);
            VisualStateManager.GoToState(TopMenu, e.NewSize.Width < 800 ? "Collapsed" : "Overflowed", true);
        }

        private void HamburgerMenuUserControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null) return;
            var index = listView.SelectedIndex;
            switch (index)
            {
                case -1:
                    return;
                default:
                    var entry = listView?.SelectedItem as Application.Entry.Models.EntryVm;
                    Model.SetEntry(entry, index);
                    /*StackPanel.DataContext = entry;
                    TopGrid.DataContext = entry;*/
                    break;
            }
        }

        private async void Username_OnLostFocus(object sender, RoutedEventArgs e)
        {
            await Model.SetFieldValue(nameof(Model.UserName), ((TextBox) sender).Text);
        }
    }
}
