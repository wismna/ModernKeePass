using System;
using System.Collections.ObjectModel;
using Windows.Storage.AccessCache;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class RecentVm : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private ISelectableModel _selectedItem;
        private ObservableCollection<RecentItemVm> _recentItems = new ObservableCollection<RecentItemVm>();

        public ObservableCollection<RecentItemVm> RecentItems
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

        public RecentVm()
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            foreach (var entry in mru.Entries)
            {
                try
                {
                    var file = mru.GetFileAsync(entry.Token, AccessCacheOptions.SuppressAccessTimeUpdate).GetAwaiter().GetResult();
                    RecentItems.Add(new RecentItemVm(entry, file));
                }
                catch (Exception)
                {
                    mru.Remove(entry.Token);
                }
            }
            if (RecentItems.Count > 0)
                SelectedItem = RecentItems[0];
        }

    }
}
