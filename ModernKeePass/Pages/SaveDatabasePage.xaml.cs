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
        public SaveDatabasePage()
        {
            this.InitializeComponent();
            DataContext = new DatabaseVm();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var app = (App)Application.Current;
            if (app.Database == null) return;
            var databaseVm = DataContext as DatabaseVm;
            if (databaseVm == null) return;
            databaseVm.IsOpen = app.Database.IsOpen;
            databaseVm.NotifyPropertyChanged("IsOpen");
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var app = (App) Application.Current;
            app.Database.Save();
            app.Database.Close();
        }
    }
}
