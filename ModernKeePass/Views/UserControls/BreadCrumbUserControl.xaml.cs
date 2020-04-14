using System.Collections.Generic;
using Windows.UI.Xaml;
using ModernKeePass.Application.Common.Interfaces;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class BreadCrumbUserControl
    {
        public BreadCrumbUserControl()
        {
            InitializeComponent();
        }

        public IEnumerable<IEntityVm> ItemsSource
        {
            get { return (IEnumerable<IEntityVm>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable<IEntityVm>),
                typeof(BreadCrumbUserControl),
                new PropertyMetadata(new Stack<IEntityVm>(), (o, args) => { }));
    }
}
