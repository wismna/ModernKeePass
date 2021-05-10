// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Xaml.Navigation;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecentDatabasesPage
    {
        public RecentVm Model => (RecentVm)DataContext;

        public RecentDatabasesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Model.Cleanup();
        }
    }
}
