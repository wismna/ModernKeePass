using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;

namespace ModernKeePass.TemplateSelectors
{
    public class SelectableDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate TrueItem { get; set; }
        public DataTemplate FalseItem { get; set; }
        
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var isSelectableItem = item as ISelectableModel;
            return isSelectableItem != null && isSelectableItem.IsSelected ? TrueItem : FalseItem;
        }
    }
}
