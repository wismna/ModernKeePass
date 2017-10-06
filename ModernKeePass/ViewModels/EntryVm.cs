using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Mappings;
using ModernKeePassLib;
using ModernKeePassLib.Security;

namespace ModernKeePass.ViewModels
{
    public class EntryVm: NotifyPropertyChangedBase
    {
        public GroupVm ParentGroup { get; }
        public PwEntry Entry { get; }

        public System.Drawing.Color? BackgroundColor => Entry?.BackgroundColor;
        public System.Drawing.Color? ForegroundColor => Entry?.ForegroundColor;

        public string Title
        {
            get
            {
                var title = GetEntryValue(PwDefs.TitleField);
                return title == null ? "New entry" : title;
            }
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

        public Symbol IconSymbol
        {
            get
            {
                if (Entry == null) return Symbol.Add;
                var result = PwIconToSegoeMapping.GetSymbolFromIcon(Entry.IconId);
                return result == Symbol.More ? Symbol.Permissions : result;
            }
        }

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
        }

        public bool IsRevealPassword
        {
            get { return _isRevealPassword; }
            set { SetProperty(ref _isRevealPassword, value); }
        }

        private bool _isEditMode;
        private bool _isRevealPassword;

        public EntryVm() { }
        public EntryVm(PwEntry entry, GroupVm parent)
        {
            Entry = entry;
            ParentGroup = parent;
        }

        public void RemoveEntry()
        {
            ParentGroup.RemoveEntry(this);
        }

        private string GetEntryValue(string key)
        {
            return Entry?.Strings.GetSafe(key).ReadString();
        }
        
        private void SetEntryValue(string key, string newValue)
        {
            Entry?.Strings.Set(key, new ProtectedString(true, newValue));
        }
    }
}
