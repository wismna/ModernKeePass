using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class SettingsDatabaseVm: NotifyPropertyChangedBase, IHasSelectableObject
    {
        private readonly App _app = (App)Application.Current;
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        private GroupVm _selectedItem;

        public bool HasRecycleBin
        {
            get { return _app.Database.RecycleBinEnabled; }
            set
            {
                _app.Database.RecycleBinEnabled = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GroupVm> Groups { get; set; }

        public ISelectableModel SelectedItem
        {
            get { return Groups.FirstOrDefault(g => g.IsSelected); }
            set
            {
                if (_selectedItem == value) return;
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                SetProperty(ref _selectedItem, (GroupVm)value);

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
            }
        }

        public SettingsDatabaseVm()
        {
            Groups = _app.Database.RootGroup.Groups;
        }

        // TODO: Move to another setting class (or a static class)
        private T GetSetting<T>(string property)
        {
            try
            {
                return (T) Convert.ChangeType(_localSettings.Values[property], typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
    }
}
