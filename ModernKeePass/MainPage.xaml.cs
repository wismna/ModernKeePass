using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = new MainVm(Frame, MenuFrame);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mainVm = DataContext as MainVm;
            mainVm.SelectedItem?.Destination.Navigate(mainVm.SelectedItem.PageType, mainVm.SelectedItem.Parameter);
        }
    }
}
