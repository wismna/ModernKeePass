using System.Collections.ObjectModel;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels
{
    public class RecentVm : NotifyPropertyChangedBase
    {
        private RecentItemVm _selectedItem;
        private ObservableCollection<RecentItemVm> _recentItems;

        public ObservableCollection<RecentItemVm> RecentItems
        {
            get { return _recentItems; }
            set { SetProperty(ref _recentItems, value); }
        }

        public RecentItemVm SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                SetProperty(ref _selectedItem, value);

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
            }
        }
    }
}
