using System;
using System.ComponentModel;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;
using ModernKeePass.Mappings;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.PasswordGenerator;
using ModernKeePassLib.Security;
using ModernKeePassLib.Cryptography;

namespace ModernKeePass.ViewModels
{
    public class EntryVm : INotifyPropertyChanged, IPwEntity
    {
        public GroupVm ParentGroup { get; private set; }

        public GroupVm PreviousGroup { get; private set; }

        public System.Drawing.Color? BackgroundColor => _pwEntry?.BackgroundColor;
        public System.Drawing.Color? ForegroundColor => _pwEntry?.ForegroundColor;
        public bool IsRevealPasswordEnabled => !string.IsNullOrEmpty(Password);
        public bool HasExpired => HasExpirationDate && _pwEntry.ExpiryTime < DateTime.Now;
        public double PasswordComplexityIndicator => QualityEstimation.EstimatePasswordBits(Password.ToCharArray());
        public bool IsFirstItem => _pwEntry == null;


        public bool UpperCasePatternSelected { get; set; } = true;
        public bool LowerCasePatternSelected { get; set; } = true;
        public bool DigitsPatternSelected { get; set; } = true;
        public bool MinusPatternSelected { get; set; }
        public bool UnderscorePatternSelected { get; set; }
        public bool SpacePatternSelected { get; set; }
        public bool SpecialPatternSelected { get; set; }
        public bool BracketsPatternSelected { get; set; }
        public string CustomChars { get; set; } = string.Empty;
        public PwUuid IdUuid => _pwEntry?.Uuid;

        public double PasswordLength
        {
            get { return _passwordLength; }
            set
            {
                _passwordLength = value;
                NotifyPropertyChanged("PasswordLength");
            }
        }
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

        public string Path
        {
            get
            {
                var path = new StringBuilder(ParentGroup.Path);
                path.Append($" > {ParentGroup.Name}");
                return path.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PwEntry _pwEntry;
        private readonly IDatabase _database;
        private bool _isEditMode;
        private bool _isRevealPassword;
        private double _passwordLength = 25;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EntryVm() { }
        
        internal EntryVm(PwEntry entry, GroupVm parent) : this(entry, parent, (Application.Current as App)?.Database) { }

        public EntryVm(PwEntry entry, GroupVm parent, IDatabase database)
        {
            _database = database;
            _pwEntry = entry;
            ParentGroup = parent;
        }
        
        public void GeneratePassword()
        {
            var pwProfile = new PwProfile
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
            if (_database.RecycleBinEnabled && _database.RecycleBin?.IdUuid == null)
                _database.CreateRecycleBin();
            Move(_database.RecycleBinEnabled && !ParentGroup.IsSelected ? _database.RecycleBin : null);
        }

        public void UndoDelete()
        {
            Move(PreviousGroup);
        }

        public void Move(GroupVm destination)
        {
            PreviousGroup = ParentGroup;
            PreviousGroup.Entries.Remove(this);
            if (destination == null)
            {
                _database.AddDeletedItem(IdUuid);
                return;
            }
            ParentGroup = destination;
            ParentGroup.Entries.Add(this);
        }
        
        public void CommitDelete()
        {
            _pwEntry.ParentGroup.Entries.Remove(_pwEntry);
            if (!_database.RecycleBinEnabled || PreviousGroup.IsSelected) _database.AddDeletedItem(IdUuid);
        }

        public void Save()
        {
            _database.Save();
        }

        public PwEntry GetPwEntry()
        {
            return _pwEntry;
        }
    }
}
