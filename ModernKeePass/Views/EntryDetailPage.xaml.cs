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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var args = e.Parameter as NavigationItem;
            if (args != null)
            {
                await Model.Initialize(args.Id);
            }
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            await Model.AddHistory();
        }

        #endregion
        
        private void EntryDetailPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 640)
            {
                VisualStateManager.GoToState(this, "Small", true);
                VisualStateManager.GoToState(TopMenu, "Collapsed", true);
                VisualStateManager.GoToState(HamburgerMenu, "Hidden", true);
            }
            else if (e.NewSize.Width > 640 && e.NewSize.Width <= 1008)
            {
                VisualStateManager.GoToState(this, "Medium", true);
                VisualStateManager.GoToState(TopMenu, "Overflowed", true);
                VisualStateManager.GoToState(HamburgerMenu, "Collapsed", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Large", true);
                VisualStateManager.GoToState(TopMenu, "Overflowed", true);
                VisualStateManager.GoToState(HamburgerMenu, "Collapsed", true);
            }
        }
    }
}
