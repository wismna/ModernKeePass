using System.ComponentModel;
using ModernKeePassLibPCL;

namespace ModernKeePass.ViewModels
{
    public class EntryVm : INotifyPropertyChanged
    {
        public string Title { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string URL { get; private set; }
        public string Notes { get; private set; }

        public EntryVm() { }
        public EntryVm(PwEntry entry)
        {
            Title = entry.Strings.GetSafe(PwDefs.TitleField).ReadString();
            UserName = entry.Strings.GetSafe(PwDefs.UserNameField).ReadString();
            Password = entry.Strings.GetSafe(PwDefs.PasswordField).ReadString();
            URL = entry.Strings.GetSafe(PwDefs.UrlField).ReadString();
            Notes = entry.Strings.GetSafe(PwDefs.NotesField).ReadString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
