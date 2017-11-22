using System.ComponentModel;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: INotifyPropertyChanged
    {
        public bool ShowPasswordBox
        {
            get { return ((App)Application.Current).Database.Status == (int) DatabaseHelper.DatabaseStatus.Opening; }
        }

        public string Name
        {
            get { return ((App)Application.Current).Database.Name; }
        }

        public OpenVm()
        {
            var app = Application.Current as App;
            if (app?.Database == null || app.Database.Status != (int) DatabaseHelper.DatabaseStatus.Opening) return;
            OpenFile(app.Database.DatabaseFile);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OpenFile(StorageFile file)
        {
            var database = ((App)Application.Current).Database;
            database.DatabaseFile = file;
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
