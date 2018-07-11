using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.PasswordGenerator;
using ModernKeePassLib.Security;
using ModernKeePassLib.Cryptography;

namespace ModernKeePass.ViewModels
{
    public class EntryVm : INotifyPropertyChanged, IPwEntity, ISelectableModel
    {
        public GroupVm ParentGroup { get; private set; }
        public GroupVm PreviousGroup { get; private set; }
        public bool IsRevealPasswordEnabled => !string.IsNullOrEmpty(Password);
        public bool HasExpired => HasExpirationDate && _pwEntry.ExpiryTime < DateTime.Now;
        public double PasswordComplexityIndicator => QualityEstimation.EstimatePasswordBits(Password?.ToCharArray());
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
        public string Id => _pwEntry?.Uuid.ToHexString();
        public bool IsRecycleOnDelete => _database.RecycleBinEnabled && !ParentGroup.IsSelected;
        public IEnumerable<IPwEntity> BreadCrumb => new List<IPwEntity>(ParentGroup.BreadCrumb) {ParentGroup};
        /// <summary>
        /// Determines if the Entry is current or from history
        /// </summary>
        public bool IsSelected { get; set; } = true;

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
            get { return GetEntryValue(PwDefs.TitleField); }
            set { SetEntryValue(PwDefs.TitleField, new ProtectedString(true, value)); }
        }


        public string UserName
        {
            get { return GetEntryValue(PwDefs.UserNameField); }
            set { SetEntryValue(PwDefs.UserNameField, new ProtectedString(true, value)); }
        }

        public string Password
        {
            get { return GetEntryValue(PwDefs.PasswordField); }
            set
            {
                SetEntryValue(PwDefs.PasswordField, new ProtectedString(true, value));
                NotifyPropertyChanged("Password");
                NotifyPropertyChanged("PasswordComplexityIndicator");
            }
        }
        
        public string Url
        {
            get { return GetEntryValue(PwDefs.UrlField); }
            set { SetEntryValue(PwDefs.UrlField, new ProtectedString(true, value)); }
        }

        public string Notes
        {
            get { return GetEntryValue(PwDefs.NotesField); }
            set { SetEntryValue(PwDefs.NotesField, new ProtectedString(true, value)); }
        }

        public int IconId
        {
            get
            {
                if (HasExpired) return (int) PwIcon.Expired;
                if (_pwEntry?.IconId != null) return (int) _pwEntry?.IconId;
                return -1;
            }
            set { _pwEntry.IconId = (PwIcon)value; }
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
            get { return IsSelected && _isEditMode; }
            set
            {
                _isEditMode = value;
                NotifyPropertyChanged("IsEditMode");
            }
        }
        
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                NotifyPropertyChanged("IsVisible");
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

        public IEnumerable<IPwEntity> History
        {
            get
            {
                var history = new Stack<EntryVm>();
                foreach (var historyEntry in _pwEntry.History)
                {
                    history.Push(new EntryVm(historyEntry, ParentGroup) {IsSelected = false});
                }
                history.Push(this);

                return history;
            }
        }


        public Color? BackgroundColor
        {
            get { return _pwEntry?.BackgroundColor; }
            set
            {
                if (value != null) _pwEntry.BackgroundColor = (Color) value;
            }
        }
        public Color? ForegroundColor
        {
            get { return _pwEntry?.ForegroundColor; }
            set
            {
                if (value != null) _pwEntry.ForegroundColor = (Color)value;
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand GeneratePasswordCommand { get; }
        public ICommand UndoDeleteCommand { get; }
        public ICommand GoBackCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly PwEntry _pwEntry;
        private readonly IDatabaseService _database;
        private readonly IResourceService _resource;
        private bool _isEditMode;
        private bool _isDirty;
        private bool _isRevealPassword;
        private double _passwordLength = 25;
        private bool _isVisible = true;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EntryVm() { }
        
        internal EntryVm(PwEntry entry, GroupVm parent) : this(entry, parent, DatabaseService.Instance, new ResourcesService()) { }

        public EntryVm(PwEntry entry, GroupVm parent, IDatabaseService database, IResourceService resource)
        {
            _database = database;
            _resource = resource;
            _pwEntry = entry;
            ParentGroup = parent;

            SaveCommand = new RelayCommand(() => _database.Save());
            GeneratePasswordCommand = new RelayCommand(GeneratePassword);
            UndoDeleteCommand = new RelayCommand(() => Move(PreviousGroup));
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

            SetEntryValue(PwDefs.PasswordField, password);
            NotifyPropertyChanged("Password");
            NotifyPropertyChanged("IsRevealPasswordEnabled");
            NotifyPropertyChanged("PasswordComplexityIndicator");
        }

        
        public void MarkForDelete(string recycleBinTitle)
        {
            if (_database.RecycleBinEnabled && _database.RecycleBin?.IdUuid == null)
                _database.CreateRecycleBin(recycleBinTitle);
            Move(_database.RecycleBinEnabled && !ParentGroup.IsSelected ? _database.RecycleBin : null);
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
        
        public PwEntry GetPwEntry()
        {
            return _pwEntry;
        }
        public void Reset()
        {
            _isDirty = false;
        }
        
        public override string ToString()
        {
            return IsSelected ? _resource.GetResourceValue("EntryCurrent") : _pwEntry.LastModificationTime.ToString("g");
        }

        private string GetEntryValue(string key)
        {
            return _pwEntry?.Strings.GetSafe(key).ReadString();
        }
        
        private void SetEntryValue(string key, ProtectedString newValue)
        {
            if (!_isDirty)
            {
                _pwEntry.Touch(true);
                _pwEntry?.CreateBackup(null);
            }
            _pwEntry?.Strings.Set(key, newValue);
            _isDirty = true;
        }
    }
}
