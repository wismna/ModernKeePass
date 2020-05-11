using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    public class RecentVm : ViewModelBase
    {
        private readonly IRecentProxy _recent;
        private ObservableCollection<RecentItemVm> _recentItems;
        private int _selectedIndex;

        public ObservableCollection<RecentItemVm> RecentItems
        {
            get { return _recentItems; }
            set { Set(() => RecentItems, ref _recentItems, value); }
        }
        
        public ICommand ClearAllCommand { get; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { Set(() => SelectedIndex, ref _selectedIndex, value); }
        }

        public RecentVm(IRecentProxy recent)
        {
            _recent = recent;
            ClearAllCommand = new RelayCommand(ClearAll);
            PopulateRecentItems();
            MessengerInstance.Register<FileNotFoundMessage>(this, _ => PopulateRecentItems());
        }

        private void PopulateRecentItems()
        {
            var recentItems = _recent.GetAll().Select(r => new RecentItemVm(r));
            RecentItems = new ObservableCollection<RecentItemVm>(recentItems);
            if (RecentItems.Any()) SelectedIndex = 0;
        }

        private void ClearAll()
        {
            _recent.ClearAll();
            RecentItems.Clear();
        }
    }
}
