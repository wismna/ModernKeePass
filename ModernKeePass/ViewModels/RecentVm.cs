using System.Collections.ObjectModel;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class RecentVm : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private ISelectableModel _selectedItem;
        private ObservableCollection<IRecentItem> _recentItems = new ObservableCollection<IRecentItem>();

        public ObservableCollection<IRecentItem> RecentItems
        {
            get { return _recentItems; }
            set { SetProperty(ref _recentItems, value); }
        }

        public ISelectableModel SelectedItem
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
                
                if (_selectedItem == null) return;
                _selectedItem.IsSelected = true;
            }
        }

        public RecentVm() : this (new RecentService())
        { }

        public RecentVm(IRecent recent)
        {
            RecentItems = recent.GetAllFiles();
            if (RecentItems.Count > 0)
                SelectedItem = RecentItems[0] as RecentItemVm;
        }

    }
}
