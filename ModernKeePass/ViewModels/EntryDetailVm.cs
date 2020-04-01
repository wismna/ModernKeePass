using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using MediatR;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Commands.SetFieldValue;
using ModernKeePass.Application.Group.Commands.AddEntry;
using ModernKeePass.Application.Group.Commands.CreateGroup;
using ModernKeePass.Application.Group.Commands.DeleteEntry;
using ModernKeePass.Application.Group.Commands.RemoveEntry;
using ModernKeePass.Application.Security.Commands.GeneratePassword;
using ModernKeePass.Application.Security.Queries.EstimatePasswordComplexity;
using ModernKeePass.Common;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class EntryDetailVm : NotifyPropertyChangedBase, IVmEntity, ISelectableModel
    {
        public bool IsRevealPasswordEnabled => !string.IsNullOrEmpty(Password);
        public bool HasExpired => HasExpirationDate && ExpiryDate < DateTime.Now;
        public double PasswordComplexityIndicator => _mediator.Send(new EstimatePasswordComplexityQuery {Password = Password}).GetAwaiter().GetResult();
        public bool UpperCasePatternSelected { get; set; } = true;
        public bool LowerCasePatternSelected { get; set; } = true;
        public bool DigitsPatternSelected { get; set; } = true;
        public bool MinusPatternSelected { get; set; }
        public bool UnderscorePatternSelected { get; set; }
        public bool SpacePatternSelected { get; set; }
        public bool SpecialPatternSelected { get; set; }
        public bool BracketsPatternSelected { get; set; }
        public string CustomChars { get; set; } = string.Empty;
        public string Id => _entry.Id;

        public IEnumerable<Application.Group.Models.GroupVm> BreadCrumb => _entry.Breadcrumb;

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
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return _entry.Title; }
            set
            {
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Title), FieldValue = value}).Wait();
                _entry.Title = value;
            }
        }

        public string UserName
        {
            get { return _entry.Username; }
            set { _entry.Username = value; }
        }

        public string Password
        {
            get { return _entry.Password; }
            set
            {
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Password), FieldValue = value }).Wait();
                _entry.Password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordComplexityIndicator));
            }
        }
        
        public string Url
        {
            get { return _entry.Url?.ToString(); }
            set
            {
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Url), FieldValue = value }).Wait();
                _entry.Url = new Uri(value);
            }
        }

        public string Notes
        {
            get { return _entry.Notes; }
            set
            {
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Notes), FieldValue = value }).Wait();
                _entry.Notes = value;
            }
        }

        public Symbol Icon
        {
            get
            {
                if (HasExpired) return Symbol.ReportHacked;
                return (Symbol) _entry.Icon;
            }
            set
            {
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Icon), FieldValue = value }).Wait();
                _entry.Icon = (Icon)value;
            }
        }

        public DateTimeOffset ExpiryDate
        {
            get { return _entry.ExpirationDate; }
            set
            {
                if (!HasExpirationDate) return;
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = "ExpirationDate", FieldValue = value.Date }).Wait();
                _entry.ExpirationDate = value.Date;
            }
        }

        public TimeSpan ExpiryTime
        {
            get { return _entry.ExpirationDate.TimeOfDay; }
            set
            {
                if (!HasExpirationDate) return;
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = "ExpirationDate", FieldValue = ExpiryDate.Date.Add(value) }).Wait();
                _entry.ExpirationDate = _entry.ExpirationDate.Date.Add(value);
            }
        }
        
        public bool HasExpirationDate
        {
            get { return _entry.HasExpirationDate; }
            set
            {
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(HasExpirationDate), FieldValue = value }).Wait();
                _entry.HasExpirationDate = value;
                OnPropertyChanged();
            }
        }

        public Color? BackgroundColor
        {
            get { return _entry?.BackgroundColor; }
            set
            {
                if (value != null)
                {
                    _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(BackgroundColor), FieldValue = value }).Wait();
                    _entry.BackgroundColor = (Color)value;
                }
            }
        }

        public Color? ForegroundColor
        {
            get { return _entry?.ForegroundColor; }
            set
            {
                if (value != null)
                {
                    _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(ForegroundColor), FieldValue = value }).Wait();
                    _entry.ForegroundColor = (Color)value;
                }
            }
        }
        public IEnumerable<Application.Entry.Models.EntryVm> History => _entry.History;

        public bool IsEditMode
        {
            get { return IsSelected && _isEditMode; }
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsRevealPassword
        {
            get { return _isRevealPassword; }
            set
            {
                _isRevealPassword = value;
                OnPropertyChanged();
            }
        }

        public bool CanRestore => _entry.ParentGroup == _database.RecycleBin;

        public ICommand SaveCommand { get; }
        public ICommand GeneratePasswordCommand { get; }
        public ICommand UndoDeleteCommand { get; }
        
        private readonly Application.Entry.Models.EntryVm _entry;
        private readonly IMediator _mediator;
        private readonly DatabaseVm _database;
        private bool _isEditMode;
        private bool _isRevealPassword;
        private double _passwordLength = 25;
        private bool _isVisible = true;

        public EntryDetailVm() { }
        
        internal EntryDetailVm(Application.Entry.Models.EntryVm entry, bool isNewEntry = false) : this(entry, App.Mediator, isNewEntry) { }

        public EntryDetailVm(Application.Entry.Models.EntryVm entry, IMediator mediator, bool isNewEntry = false)
        {
            _entry = entry;
            _mediator = mediator;
            _database = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            _isEditMode = isNewEntry;
            if (isNewEntry) GeneratePassword().GetAwaiter().GetResult();

            SaveCommand = new RelayCommand(() => _mediator.Send(new SaveDatabaseCommand()));
            GeneratePasswordCommand = new RelayCommand(async () => await GeneratePassword());
            UndoDeleteCommand = new RelayCommand(async () => await Move(entry.ParentGroup), () => _entry.ParentGroup != null);
        }
        
        public async Task GeneratePassword()
        {
            Password = await _mediator.Send(new GeneratePasswordCommand
            {
                BracketsPatternSelected = BracketsPatternSelected,
                CustomChars = CustomChars,
                DigitsPatternSelected = DigitsPatternSelected,
                LowerCasePatternSelected = LowerCasePatternSelected,
                MinusPatternSelected = MinusPatternSelected,
                PasswordLength = (int)PasswordLength,
                SpacePatternSelected = SpacePatternSelected,
                SpecialPatternSelected = SpecialPatternSelected,
                UnderscorePatternSelected = UnderscorePatternSelected,
                UpperCasePatternSelected = UpperCasePatternSelected
            });
            OnPropertyChanged(nameof(IsRevealPasswordEnabled));
        }

        public async Task MarkForDelete(string recycleBinTitle)
        {
            if (_database.IsRecycleBinEnabled && _database.RecycleBin == null)
                _database.RecycleBin = await _mediator.Send(new CreateGroupCommand { ParentGroup = _database.RootGroup, IsRecycleBin = true, Name = recycleBinTitle});
            await Move(_database.IsRecycleBinEnabled && _entry.ParentGroup != _database.RecycleBin ? _database.RecycleBin : null);
        }
        
        public async Task Move(Application.Group.Models.GroupVm destination)
        {
            await _mediator.Send(new AddEntryCommand { ParentGroup = destination, Entry = _entry });
            await _mediator.Send(new RemoveEntryCommand { ParentGroup = _entry.ParentGroup, Entry = _entry });
            if (destination == null)
            {
                await _mediator.Send(new DeleteEntryCommand { Entry = _entry });
            }
        }
        
        public async Task CommitDelete()
        {
            await _mediator.Send(new DeleteEntryCommand {Entry = _entry});
        }
        
        public Application.Entry.Models.EntryVm GetEntry()
        {
            return _entry;
        }

        public async Task SetFieldValue(string fieldName, object value)
        {
            await _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = fieldName, FieldValue = value });
        }
    }
}
