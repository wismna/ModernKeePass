using Windows.Storage;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: NotifyPropertyChangedBase
    {
        private readonly IRecentService _recent;
        public bool IsFileSelected => DatabaseFile != null;

        public string Name => DatabaseFile?.DisplayName;

        public StorageFile DatabaseFile { get; private set; }

        public OpenVm(): this(new RecentService()) { }

        public OpenVm(IRecentService recent)
        {
            _recent = recent;
        }
        
        public void OpenFile(StorageFile file)
        {
            DatabaseFile = file;
            OnPropertyChanged("Name");
            OnPropertyChanged("IsFileSelected");
            OnPropertyChanged("DatabaseFile");
            AddToRecentList(file);
        }
        
        private void AddToRecentList(StorageFile file)
        {
            _recent.Add(file, file.DisplayName);
        }
    }
}
