using Windows.UI.Xaml.Controls;
using ModernKeePass.Mappings;
using ModernKeePassLib;
using ModernKeePassLib.Security;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;

namespace ModernKeePass.ViewModels
{
    public class EntryVm
    {
        public GroupVm ParentGroup { get; }
        public PwEntry Entry => _pwEntry;
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
        public System.Drawing.Color? BackgroundColor => _pwEntry?.BackgroundColor;

        public System.Drawing.Color? ForegroundColor => _pwEntry?.ForegroundColor;
        
        public FontWeight FontWeight => _pwEntry == null ? FontWeights.Bold : FontWeights.Normal;

        public Symbol IconSymbol
        {
            get
            {
                if (_pwEntry == null) return Symbol.Add;
                var result = PwIconToSegoeMapping.GetSymbolFromIcon(_pwEntry.IconId);
                return result == Symbol.More ? Symbol.Permissions : result;
            }
        }
        
        private readonly PwEntry _pwEntry;

        public EntryVm() { }
        public EntryVm(PwEntry entry, GroupVm parent)
        {
            _pwEntry = entry;
            ParentGroup = parent;
        }

        public void RemoveEntry()
        {
            ParentGroup.RemoveEntry(this);
        }

        private string GetEntryValue(string key)
        {
            return _pwEntry?.Strings.GetSafe(key).ReadString();
        }
        
        private void SetEntryValue(string key, string newValue)
        {
            _pwEntry?.Strings.Set(key, new ProtectedString(true, newValue));
        }
    }
}
