using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SaveDatabasePage : Page
    {
        private Frame _mainFrame;
        public SaveDatabasePage()
        {
            InitializeComponent();
            DataContext = new DatabaseVm();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainFrame = e.Parameter as Frame;
            var app = (App)Application.Current;
            if (app.Database == null) return;
            var databaseVm = DataContext as DatabaseVm;
            if (databaseVm == null) return;
            UpdateDatabaseStatus(app, databaseVm);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var app = (App) Application.Current;
            app.Database.Save();
            app.Database.Close();
            UpdateDatabaseStatus(app, DataContext as DatabaseVm);
            _mainFrame.Navigate(typeof(MainPage));
        }

        private void UpdateDatabaseStatus(App app, DatabaseVm databaseVm)
        {
            databaseVm.IsOpen = app.Database.IsOpen;
            databaseVm.NotifyPropertyChanged("IsOpen");
        }
    }
}
