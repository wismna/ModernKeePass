using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Events;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Pages
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecentDatabasesPage : Page
    {
        private Frame _mainFrame;

        public RecentDatabasesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainFrame = e.Parameter as Frame;
        }
        
        private void PasswordUserControl_PasswordChecked(object sender, PasswordEventArgs e)
        {
            _mainFrame.Navigate(typeof(GroupDetailPage), e.RootGroup);
        }

        private void OpenDatabaseUserControl_OnValidationChecking(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            var viewModel = DataContext as RecentVm;
            var app = (App)Application.Current;
            app.Database.DatabaseFile = viewModel.SelectedItem.DatabaseFile;
        }
    }
}
