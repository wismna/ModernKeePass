using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Controls
{
    public class ListViewWithDisable: ListView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var binaryItem = item as IIsEnabled;
            if (!(element is ListViewItem container) || binaryItem == null) return;
            container.IsEnabled = binaryItem.IsEnabled;
            container.IsHitTestVisible = binaryItem.IsEnabled;
        }
    }
}
