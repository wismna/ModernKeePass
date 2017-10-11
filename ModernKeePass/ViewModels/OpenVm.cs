using System.ComponentModel;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: INotifyPropertyChanged
    {
        public StorageFile File { get; set; }
        public bool ShowPasswordBox
        {
            get { return File != null; }
        }

        public string Name
        {
            get { return File?.Name; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OpenFile(StorageFile file)
        {
            File = file;
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("ShowPasswordBox");
            AddToRecentList(file);
        }
        
        private void AddToRecentList(StorageFile file)
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            mru.Add(file, file.DisplayName);
        }
    }
}
