using System.Linq;
using Windows.UI.Xaml;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class BreadcrumbUserControl
    {
        public GroupVm Group
        {
            get { return (GroupVm)GetValue(GroupProperty); }
            set { SetValue(GroupProperty, value); }
        }
        public static readonly DependencyProperty GroupProperty =
            DependencyProperty.Register(
                nameof(Group),
                typeof(GroupVm),
                typeof(BreadcrumbUserControl),
                new PropertyMetadata(null, async (o, args) =>
                {
                    var userControl = o as BreadcrumbUserControl;
                    var vm = userControl?.StackPanel.DataContext as BreadcrumbControlVm;
                    if (vm == null) return;
                    await vm.Initialize(args.NewValue as GroupVm).ConfigureAwait(false);
                    userControl?.ListView.ScrollIntoView(vm.BreadcrumbItems.Last());
                }));

        public BreadcrumbUserControl()
        {
            InitializeComponent();
        }
    }
}
