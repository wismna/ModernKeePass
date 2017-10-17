using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;
using ModernKeePass.Mappings;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.PasswordGenerator;
using ModernKeePassLib.Security;
using System;

namespace ModernKeePass.ViewModels
{
    public class EntryVm : INotifyPropertyChanged, IPwEntity
    {
        public GroupVm ParentGroup { get; }
        public PwEntry Entry { get; }

        public System.Drawing.Color? BackgroundColor => Entry?.BackgroundColor;
        public System.Drawing.Color? ForegroundColor => Entry?.ForegroundColor;
        public bool IsRevealPasswordEnabled => !string.IsNullOrEmpty(Password);

        public double PasswordLength { get; set; } = 25;
        public bool UpperCasePatternSelected { get; set; } = true;
        public bool LowerCasePatternSelected { get; set; } = true;
        public bool DigitsPatternSelected { get; set; } = true;
        public bool MinusPatternSelected { get; set; }
        public bool UnderscorePatternSelected { get; set; }
        public bool SpacePatternSelected { get; set; }
        public bool SpecialPatternSelected { get; set; }
        public bool BracketsPatternSelected { get; set; }
        public string CustomChars { get; set; } = string.Empty;

        public string Name
        {
            get
            {
                var title = GetEntryValue(PwDefs.TitleField);
                return title == null ? "New entry" : title;
            }
            set { SetEntryValue(PwDefs.TitleField, value); }
        }

        public string Id => Entry.Uuid.ToHexString();

        public string UserName
        {
            get { return GetEntryValue(PwDefs.UserNameField); }
            set { SetEntryValue(PwDefs.UserNameField, value); }
        }
        public string Password
        {
            get { return GetEntryValue(PwDefs.PasswordField); }
            set
            {
                SetEntryValue(PwDefs.PasswordField, value);
                NotifyPropertyChanged("Password");
            }
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
            set
            {
                _isEditMode = value;
                NotifyPropertyChanged("IsEditMode");
            }
        }

        public bool IsRevealPassword
        {
            get { return _isRevealPassword; }
            set
            {
                _isRevealPassword = value;
                NotifyPropertyChanged("IsRevealPassword");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isEditMode;
        private bool _isRevealPassword;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public void GeneratePassword()
        {
            var pwProfile = new PwProfile()
            {
                GeneratorType = PasswordGeneratorType.CharSet,
                Length = (uint)PasswordLength,
                CharSet = new PwCharSet()
            };

            if (UpperCasePatternSelected) pwProfile.CharSet.Add(PwCharSet.UpperCase);
            if (LowerCasePatternSelected) pwProfile.CharSet.Add(PwCharSet.LowerCase);
            if (DigitsPatternSelected) pwProfile.CharSet.Add(PwCharSet.Digits);
            if (SpecialPatternSelected) pwProfile.CharSet.Add(PwCharSet.SpecialChars);
            if (MinusPatternSelected) pwProfile.CharSet.Add('-');
            if (UnderscorePatternSelected) pwProfile.CharSet.Add('_');
            if (SpacePatternSelected) pwProfile.CharSet.Add(' ');
            if (BracketsPatternSelected) pwProfile.CharSet.Add(PwCharSet.Brackets);

            pwProfile.CharSet.Add(CustomChars);

            ProtectedString password;
            PwGenerator.Generate(out password, pwProfile, null, new CustomPwGeneratorPool());

            Entry?.Strings.Set(PwDefs.PasswordField, password);
            NotifyPropertyChanged("Password");
            NotifyPropertyChanged("IsRevealPasswordEnabled");
        }

        private string GetEntryValue(string key)
        {
            return Entry?.Strings.GetSafe(key).ReadString();
        }
        
        private void SetEntryValue(string key, string newValue)
        {
            Entry?.Strings.Set(key, new ProtectedString(true, newValue));
        }

        public void CommitDelete()
        {
            throw new NotImplementedException();
        }
    }
}
