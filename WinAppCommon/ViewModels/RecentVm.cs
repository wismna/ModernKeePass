using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    public class RecentVm : ViewModelBase, IHasSelectableObject
    {
        private readonly IRecentProxy _recent;
        private ISelectableModel _selectedItem;
        private ObservableCollection<RecentItemVm> _recentItems;

        public ObservableCollection<RecentItemVm> RecentItems
        {
            get { return _recentItems; }
            set { Set(() => RecentItems, ref _recentItems, value); }
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

                Set(() => SelectedItem, ref _selectedItem, value);

                if (_selectedItem == null) return;
                _selectedItem.IsSelected = true;
            }
        }
        
        public ICommand ClearAllCommand { get; }
        
        public RecentVm(IRecentProxy recent)
        {
            _recent = recent;
            ClearAllCommand = new RelayCommand(ClearAll);
            
            var recentItems = _recent.GetAll().Select(r => new RecentItemVm(r));
            RecentItems = new ObservableCollection<RecentItemVm>(recentItems);
            if (RecentItems.Count > 0)
                SelectedItem = RecentItems[0];

            MessengerInstance.Register<DatabaseOpeningMessage>(this, async action => await UpdateAccessTime(action.Token));
        }
        
        private async Task UpdateAccessTime(string token)
        {
            await _recent.Get(token, true);
        }

        private void ClearAll()
        {
            _recent.ClearAll();
            RecentItems.Clear();
        }
    }
}
