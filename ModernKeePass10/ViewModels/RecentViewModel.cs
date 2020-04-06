using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Autofac;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    public class RecentViewModel : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private readonly IRecentService _recentService;
        private ISelectableModel _selectedItem;
        private ObservableCollection<RecentItemViewModel> _recentItems = new ObservableCollection<RecentItemViewModel>();

        public ObservableCollection<RecentItemViewModel> RecentItems
        {
            get => _recentItems;
            set => SetProperty(ref _recentItems, value);
        }

        public ISelectableModel SelectedItem
        {
            get => _selectedItem;
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

        public RecentViewModel() : this (App.Container.Resolve<IRecentService>())
        { }

        public RecentViewModel(IRecentService recentService)
        {
            _recentService = recentService;
            ClearAllCommand = new RelayCommand(ClearAll);

            RecentItems = new ObservableCollection<RecentItemViewModel>(
                _recentService.GetAll().GetAwaiter().GetResult().Select(r => new RecentItemViewModel
                {
                    Name = r.Name,
                    Token = r.Path
                })
            );
            if (RecentItems.Count > 0) SelectedItem = RecentItems[0];
        }

        private void ClearAll()
        {
            _recentService.ClearAll();
            RecentItems.Clear();
        }
    }
}
