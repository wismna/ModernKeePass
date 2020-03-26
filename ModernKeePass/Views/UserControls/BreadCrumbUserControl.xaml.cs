using System.Collections.Generic;
using Windows.UI.Xaml;
using ModernKeePass.Interfaces;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class BreadCrumbUserControl
    {
        public BreadCrumbUserControl()
        {
            InitializeComponent();
        }

        public IEnumerable<IVmEntity> ItemsSource
        {
            get { return (IEnumerable<IVmEntity>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable<IVmEntity>),
                typeof(BreadCrumbUserControl),
                new PropertyMetadata(new Stack<IVmEntity>(), (o, args) => { }));
    }
}
