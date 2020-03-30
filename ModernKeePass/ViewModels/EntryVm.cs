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
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class EntryVm : NotifyPropertyChangedBase, IVmEntity, ISelectableModel
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

        public IEnumerable<Application.Group.Models.GroupVm> BreadCrumb
        {
            get
            {
                var groups = new Stack<Application.Group.Models.GroupVm>();
                var group = _entry.ParentGroup;
                while (group.ParentGroup != null)
                {
                    group = group.ParentGroup;
                    groups.Push(group);
                }

                return groups;
            }
        }

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
            set { _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Title), FieldValue = value}); }
        }


        public string UserName
        {
            get { return _entry.Username; }
            set { _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(UserName), FieldValue = value }); }
        }

        public string Password
        {
            get { return _entry.Password; }
            set
            {
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Password), FieldValue = value });
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordComplexityIndicator));
            }
        }
        
        public string Url
        {
            get { return _entry.Url.ToString();}
            set { _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Url), FieldValue = value }); }
        }

        public string Notes
        {
            get { return _entry.Notes; }
            set { _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Notes), FieldValue = value }); }
        }

        public Symbol Icon
        {
            get
            {
                if (HasExpired) return Symbol.ReportHacked;
                return (Symbol) _entry.Icon;
            }
            set { _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = nameof(Icon), FieldValue = value }); }
        }

        public DateTimeOffset ExpiryDate
        {
            get { return _entry.ExpirationDate; }
            set
            {
                if (!HasExpirationDate) return;
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = "ExpirationDate", FieldValue = value.Date });
            }
        }

        public TimeSpan ExpiryTime
        {
            get { return _entry.ExpirationDate.TimeOfDay; }
            set
            {
                if (!HasExpirationDate) return;
                _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = "ExpirationDate", FieldValue = ExpiryDate.Date.Add(value) });
            }
        }

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
        public bool HasExpirationDate
        {
            get { return _entry.HasExpirationDate; }
            set
            {
                _mediator.Send(new SetFieldValueCommand {EntryId = Id, FieldName = nameof(HasExpirationDate), FieldValue = value});
                OnPropertyChanged();
            }
        }

        public IEnumerable<Application.Entry.Models.EntryVm> History => _entry.History;


        public Color? BackgroundColor
        {
            get { return _entry?.BackgroundColor; }
            set
            {
                if (value != null) _entry.BackgroundColor = (Color) value;
            }
        }

        public Color? ForegroundColor
        {
            get { return _entry?.ForegroundColor; }
            set
            {
                if (value != null) _entry.ForegroundColor = (Color)value;
            }
        }

        public bool CanRestore => _entry.ParentGroup == _database.RecycleBin;

        public ICommand SaveCommand { get; }
        public ICommand GeneratePasswordCommand { get; }
        public ICommand UndoDeleteCommand { get; }
        
        private readonly Application.Entry.Models.EntryVm _entry;
        private readonly IMediator _mediator;
        private readonly IResourceService _resource;
        private readonly DatabaseVm _database;
        private bool _isEditMode;
        private bool _isRevealPassword;
        private double _passwordLength = 25;
        private bool _isVisible = true;

        public EntryVm() { }
        
        internal EntryVm(Application.Entry.Models.EntryVm entry, bool isNewEntry = false) : this(entry, App.Mediator, new ResourcesService(), isNewEntry) { }

        public EntryVm(Application.Entry.Models.EntryVm entry, IMediator mediator, IResourceService resource, bool isNewEntry = false)
        {
            _entry = entry;
            _mediator = mediator;
            _resource = resource;
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
                await _mediator.Send(new CreateGroupCommand { ParentGroup = _database.RootGroup, IsRecycleBin = true, Name = recycleBinTitle});
            await Move(_database.IsRecycleBinEnabled && _entry.ParentGroup == _database.RecycleBin ? _database.RecycleBin : null);
        }
        
        public async Task Move(Application.Group.Models.GroupVm destination)
        {
            await _mediator.Send(new RemoveEntryCommand { ParentGroup = _entry.ParentGroup, Entry = _entry });
            if (destination == null)
            {
                await _mediator.Send(new DeleteEntryCommand { Entry = _entry });
                return;
            }
            await _mediator.Send(new AddEntryCommand { ParentGroup = destination, Entry = _entry });
        }
        
        public async Task CommitDelete()
        {
            await _mediator.Send(new DeleteEntryCommand {Entry = _entry});
        }
        
        public Application.Entry.Models.EntryVm GetEntry()
        {
            return _entry;
        }
        
        public override string ToString()
        {
            return IsSelected ? 
                _resource.GetResourceValue("EntryCurrent") : 
                _entry.ModificationDate.ToString("g");
        }
    }
}
