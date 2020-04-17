using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Controls
{
    public class ListViewWithDisable: ListView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var container = element as ListViewItem;
            var binaryItem = item as IIsEnabled;
            if (container == null || binaryItem == null) return;
            container.IsEnabled = binaryItem.IsEnabled;
            container.IsHitTestVisible = binaryItem.IsEnabled;
        }
    }
}
