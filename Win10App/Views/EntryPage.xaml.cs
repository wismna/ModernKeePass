using Windows.UI.Xaml.Navigation;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.Views
{
    public partial class EntryPage
    {
        public EntryItemVm Vm { get; set; }

        public EntryPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is EntryItemVm entry) Vm = entry;
        }
    }
}