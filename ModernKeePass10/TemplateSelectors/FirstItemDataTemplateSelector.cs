using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModernKeePass.TemplateSelectors
{
    public class FirstItemDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate FirstItem { get; set; }
        public DataTemplate OtherItem { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            var returnTemplate = itemsControl?.IndexFromContainer(container) == 0 ? FirstItem : OtherItem;
            return returnTemplate;
        }
    }
}