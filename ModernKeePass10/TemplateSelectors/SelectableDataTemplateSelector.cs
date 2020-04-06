using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.TemplateSelectors
{
    public class SelectableDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate TrueItem { get; set; }
        public DataTemplate FalseItem { get; set; }
        
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return item is ISelectableModel isSelectableItem && isSelectableItem.IsSelected ? TrueItem : FalseItem;
        }
    }
}
