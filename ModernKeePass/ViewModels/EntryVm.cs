using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;
using ModernKeePass.Mappings;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.PasswordGenerator;
using ModernKeePassLib.Security;
using System;
using Windows.UI.Xaml;
using ModernKeePassLib.Cryptography;

namespace ModernKeePass.ViewModels
{
    public class EntryVm : INotifyPropertyChanged, IPwEntity
    {
        public GroupVm ParentGroup { get; }

        public System.Drawing.Color? BackgroundColor => _pwEntry?.BackgroundColor;
        public System.Drawing.Color? ForegroundColor => _pwEntry?.ForegroundColor;
        public bool IsRevealPasswordEnabled => !string.IsNullOrEmpty(Password);
        public bool HasExpired => HasExpirationDate && _pwEntry.ExpiryTime < DateTime.Now;
        public double PasswordComplexityIndicator => QualityEstimation.EstimatePasswordBits(Password.ToCharArray());
        public bool IsFirstItem => _pwEntry == null;

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
                return title == null ? "< New entry >" : title;
            }
            set { SetEntryValue(PwDefs.TitleField, value); }
        }

        public string Id => _pwEntry?.Uuid.ToHexString();

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
                NotifyPropertyChanged("PasswordComplexityIndicator");
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
                if (_pwEntry == null) return Symbol.Add;
                if (HasExpired) return Symbol.Priority;
                var result = PwIconToSegoeMapping.GetSymbolFromIcon(_pwEntry.IconId);
                return result == Symbol.More ? Symbol.Permissions : result;
            }
        }

        public DateTimeOffset ExpiryDate
        {
            get { return new DateTimeOffset(_pwEntry.ExpiryTime.Date); }
            set { if (HasExpirationDate) _pwEntry.ExpiryTime = value.DateTime; }
        }
        public TimeSpan ExpiryTime
        {
            get { return _pwEntry.ExpiryTime.TimeOfDay; }
            set { if (HasExpirationDate) _pwEntry.ExpiryTime = _pwEntry.ExpiryTime.Date.Add(value); }
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
        public bool HasExpirationDate
        {
            get { return _pwEntry.Expires; }
            set
            {
                _pwEntry.Expires = value;
                NotifyPropertyChanged("HasExpirationDate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PwEntry _pwEntry;
        private bool _isEditMode;
        private bool _isRevealPassword;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EntryVm() { }
        public EntryVm(PwEntry entry, GroupVm parent)
        {
            _pwEntry = entry;
            ParentGroup = parent;
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

            _pwEntry?.Strings.Set(PwDefs.PasswordField, password);
            NotifyPropertyChanged("Password");
            NotifyPropertyChanged("IsRevealPasswordEnabled");
            NotifyPropertyChanged("PasswordComplexityIndicator");
        }

        private string GetEntryValue(string key)
        {
            return _pwEntry?.Strings.GetSafe(key).ReadString();
        }
        
        private void SetEntryValue(string key, string newValue)
        {
            _pwEntry?.Strings.Set(key, new ProtectedString(true, newValue));
        }
        
        public void MarkForDelete()
        {
            var app = (App)Application.Current;
            app.PendingDeleteEntities.Add(Id, this);
            ParentGroup.Entries.Remove(this);
        }
        public void CommitDelete()
        {
            _pwEntry.ParentGroup.Entries.Remove(_pwEntry);
        }

        public void UndoDelete()
        {
            ParentGroup.Entries.Add(this);
        }

        public void Save()
        {
            var app = (App)Application.Current;
            app.Database.Save();
        }
    }
}
