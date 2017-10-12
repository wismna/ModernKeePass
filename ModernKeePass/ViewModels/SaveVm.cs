using System.ComponentModel;
using Windows.UI.Xaml;
using ModernKeePass.Common;
using System;
using Windows.Storage;

namespace ModernKeePass.ViewModels
{
    public class SaveVm: INotifyPropertyChanged
    {
        public bool IsSaveEnabled
        {
            get
            {
                var app = (App)Application.Current;
                return app.Database.Status == DatabaseHelper.DatabaseStatus.Opened;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save(bool close = true)
        {
            var app = (App)Application.Current;
            app.Database.Save();
            if (!close) return;
            app.Database.Close();
            NotifyPropertyChanged("IsSaveEnabled");
        }

        internal void Save(StorageFile file)
        {
            var app = (App)Application.Current;
            app.Database.Save(file);
            NotifyPropertyChanged("IsSaveEnabled");
        }
    }
}