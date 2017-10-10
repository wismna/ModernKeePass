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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainFrame = e.Parameter as Frame;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as SaveVm;
            viewModel.Save();
            _mainFrame.Navigate(typeof(MainPage));
        }
    }
}
