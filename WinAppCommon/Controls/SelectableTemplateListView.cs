using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModernKeePass.Controls
{
    public class SelectableTemplateListView: ListView
    {
        public DataTemplate SelectedItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectedItemTemplateProperty); }
            set { SetValue(SelectedItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemTemplateProperty =
            DependencyProperty.Register(
                nameof(SelectedItemTemplate),
                typeof(DataTemplate),
                typeof(PasswordBoxWithButton),
                new PropertyMetadata(null, (o, args) => { }));

        public SelectableTemplateListView()
        {
            SelectionChanged += SelectableTemplateListView_SelectionChanged;
        }
        
        private void SelectableTemplateListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null) return;
            
            foreach (var item in e.AddedItems)
            {
                var listViewItem = listView.ContainerFromItem(item) as ListViewItem;
                if (listViewItem != null) listViewItem.ContentTemplate = SelectedItemTemplate;
            }
            //Remove DataTemplate for unselected items
            foreach (var item in e.RemovedItems)
            {
                var listViewItem = listView.ContainerFromItem(item) as ListViewItem;
                if (listViewItem != null) listViewItem.ContentTemplate = ItemTemplate;
            }
        }
    }
}