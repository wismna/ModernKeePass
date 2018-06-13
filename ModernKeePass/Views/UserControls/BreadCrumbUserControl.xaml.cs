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

        public IEnumerable<IPwEntity> ItemsSource
        {
            get { return (IEnumerable<IPwEntity>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable<IPwEntity>),
                typeof(BreadCrumbUserControl),
                new PropertyMetadata(new Stack<IPwEntity>(), (o, args) => { }));
    }
}
