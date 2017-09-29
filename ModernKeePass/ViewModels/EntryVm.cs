using System.ComponentModel;
using System.Drawing;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Mappings;
using ModernKeePassLib;
using ModernKeePassLib.Security;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace ModernKeePass.ViewModels
{
    public class EntryVm
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

        public SolidColorBrush BackgroundColor => CreateFromColor(_pwEntry.BackgroundColor, Colors.Transparent);

        public SolidColorBrush ForegroundColor => CreateFromColor(_pwEntry.ForegroundColor, Colors.White);

        public Symbol IconSymbol
        {
            get
            {
                var result = PwIconToSegoeMapping.GetSymbolFromIcon(_pwEntry.IconId);
                return result == Symbol.More ? Symbol.Permissions : result;
            }
        }
        
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

        private SolidColorBrush CreateFromColor(System.Drawing.Color color, Windows.UI.Color defaultValue)
        {
            if (color == System.Drawing.Color.Empty) return new SolidColorBrush(defaultValue);
            return new SolidColorBrush(Windows.UI.Color.FromArgb(
                color.A,
                color.R,
                color.G,
                color.B));
        }
    }
}
