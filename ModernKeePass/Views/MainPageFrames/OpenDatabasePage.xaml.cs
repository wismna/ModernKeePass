using Windows.UI.Xaml.Navigation;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenDatabasePage
    {
        private OpenVm Model => (OpenVm)DataContext;
        
        public OpenDatabasePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var file = e.Parameter as FileInfo;
            Model.SetFileInformation(file);
        }
    }
}
