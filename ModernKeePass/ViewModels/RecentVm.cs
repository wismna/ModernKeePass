using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage.AccessCache;
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

                var mru = StorageApplicationPermissions.MostRecentlyUsedList;
                _selectedItem.File = mru.GetFileAsync(SelectedItem.Token).GetAwaiter().GetResult();
            }
        }

        public RecentVm()
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            RecentItems = new ObservableCollection<RecentItemVm>(
                from entry in mru.Entries
                select new RecentItemVm { Name = entry.Metadata, Token = entry.Token });
            if (RecentItems.Count > 0)
                SelectedItem = RecentItems[0];
        }

    }
}
