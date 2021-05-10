// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Xaml.Navigation;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewDatabasePage
    {
        public NewVm Model => (NewVm)DataContext;

        public NewDatabasePage()
        {
            InitializeComponent();
        }
        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Model.Cleanup();
        }
    }
}
