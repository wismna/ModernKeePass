using System.Collections.Generic;
using Windows.UI.Xaml;
using ModernKeePass.Domain.Entities;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class BreadCrumbUserControl
    {
        public BreadCrumbUserControl()
        {
            InitializeComponent();
        }

        public IEnumerable<Entity> ItemsSource
        {
            get => (IEnumerable<Entity>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable<Entity>),
                typeof(BreadCrumbUserControl),
                new PropertyMetadata(new Stack<Entity>(), (o, args) => { }));
    }
}
