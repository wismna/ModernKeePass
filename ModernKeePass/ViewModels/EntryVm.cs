using System.ComponentModel;
using ModernKeePassLibPCL;
using ModernKeePassLibPCL.Security;

namespace ModernKeePass.ViewModels
{
    public class EntryVm : INotifyPropertyChanged
    {
        public string Title
        {
            get { return GetEntryValue(PwDefs.TitleField); }
            set { SetEntryValue(PwDefs.TitleField, value); }
        }
        public string UserName
        {
            get { return GetEntryValue(PwDefs.UserNameField); }
            set { SetEntryValue(PwDefs.UserNameField, value); }
        }
        public string Password
        {
            get { return GetEntryValue(PwDefs.PasswordField); }
            set { SetEntryValue(PwDefs.PasswordField, value); }
        }
        public string Url
        {
            get { return GetEntryValue(PwDefs.UrlField); }
            set { SetEntryValue(PwDefs.UrlField, value); }
        }
        public string Notes
        {
            get { return GetEntryValue(PwDefs.NotesField); }
            set { SetEntryValue(PwDefs.NotesField, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PwEntry _pwEntry;

        public EntryVm() { }
        public EntryVm(PwEntry entry)
        {
            _pwEntry = entry;
        }

        private string GetEntryValue(string key)
        {
            return _pwEntry.Strings.GetSafe(key).ReadString();
        }
        
        private void SetEntryValue(string key, string newValue)
        {
            _pwEntry.Strings.Set(key, new ProtectedString(true, newValue));
        }
    }
}
