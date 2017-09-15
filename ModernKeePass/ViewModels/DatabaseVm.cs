using System.ComponentModel;
using Windows.Storage;

using ModernKeePassLib;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Interfaces;
using Windows.UI.Xaml;
using System;

namespace ModernKeePass.ViewModels
{
    public class DatabaseVm : INotifyPropertyChanged
    {
        private PwDatabase database = new PwDatabase();
        private StorageFile databaseFile;

        public string Password { get; set; }
        public bool IsOpen { get; set; }
        public Visibility Visibility { get; private set; }
        public string ErrorMessage { get; set; }
        public string Name { get; set; }
        public GroupVm RootGroup { get; set; }


        public DatabaseVm()
        {
            Visibility = Visibility.Collapsed;
        }
        public DatabaseVm(StorageFile databaseFile)
        {
            this.databaseFile = databaseFile;
            Visibility = Visibility.Visible;
        }
        public async void Open()
        {
            var key = new CompositeKey();
            try
            {
                key.AddUserKey(new KcpPassword(Password));
                await database.Open(IOConnectionInfo.FromFile(databaseFile), key, new NullStatusLogger());
                IsOpen = database.IsOpen;
                Name = databaseFile.DisplayName;
                RootGroup = new GroupVm (database.RootGroup);
            }
            catch (ArgumentNullException)
            {
                ErrorMessage = "Password cannot be empty";
                NotifyPropertyChanged("ErrorMessage");
            }
            catch (InvalidCompositeKeyException)
            {
                ErrorMessage = "Wrong password";
                NotifyPropertyChanged("ErrorMessage");
            }
            finally
            {
                // TODO: move this when implementing write mode
                database.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
