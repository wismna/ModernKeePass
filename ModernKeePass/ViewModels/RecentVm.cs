using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    public class RecentVm : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private readonly IRecentProxy _recent;
        private ISelectableModel _selectedItem;
        private ObservableCollection<RecentItemVm> _recentItems;

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
        
        public ICommand ClearAllCommand { get; }

        public RecentVm() : this (App.Services.GetRequiredService<IRecentProxy>())
        { }

        public RecentVm(IRecentProxy recent)
        {
            _recent = recent;
            ClearAllCommand = new RelayCommand(ClearAll);
            
            var recentItems = _recent.GetAll().Select(r => new RecentItemVm(r));
            RecentItems = new ObservableCollection<RecentItemVm>(recentItems);
            if (RecentItems.Count > 0)
                SelectedItem = RecentItems[0];
        }

        private void ClearAll()
        {
            _recent.ClearAll();
            RecentItems.Clear();
        }
    }
}
