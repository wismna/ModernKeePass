using System.Collections.ObjectModel;
using System.Windows.Input;
using ModernKeePass.Common;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class RecentVm : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private readonly IRecentService _recent;
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
        
        public ICommand ClearAllCommand { get; }

        public RecentVm() : this (RecentService.Instance)
        { }

        public RecentVm(IRecentService recent)
        {
            _recent = recent;
            ClearAllCommand = new RelayCommand(ClearAll);

            RecentItems = _recent.GetAllFiles();
            if (RecentItems.Count > 0)
                SelectedItem = RecentItems[0] as RecentItemVm;
        }

        private void ClearAll()
        {
            _recent.ClearAll();
            RecentItems.Clear();
        }
    }
}
