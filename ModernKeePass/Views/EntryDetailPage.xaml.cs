using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
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
        
        public EntryDetailPage()
        {
            InitializeComponent();
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
        }

        #endregion
        
        private void EntryDetailPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, e.NewSize.Width < 700 ? "Small" : "Large", true);
            VisualStateManager.GoToState(TopMenu, e.NewSize.Width < 800 ? "Collapsed" : "Overflowed", true);
        }
    }
}
