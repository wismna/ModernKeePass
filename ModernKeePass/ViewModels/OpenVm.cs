using Windows.Storage;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: NotifyPropertyChangedBase
    {
        public bool IsFileSelected => DatabaseFile != null;

        public string Name => DatabaseFile?.DisplayName;

        public StorageFile DatabaseFile { get; private set; }
        
        internal void OpenFile(StorageFile file)
        {
            OpenFile(file, RecentService.Instance);
        }

        public void OpenFile(StorageFile file, IRecentService recent)
        {
            DatabaseFile = file;
            OnPropertyChanged("Name");
            OnPropertyChanged("IsFileSelected");
            OnPropertyChanged("DatabaseFile");
            AddToRecentList(file, recent);
        }
        
        private void AddToRecentList(StorageFile file, IRecentService recent)
        {
            recent.Add(file, file.DisplayName);
        }
    }
}
