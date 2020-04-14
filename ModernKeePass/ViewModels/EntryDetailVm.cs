using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Commands.SetFieldValue;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Entry.Queries.GetEntry;
using ModernKeePass.Application.Group.Commands.AddEntry;
using ModernKeePass.Application.Group.Commands.DeleteEntry;
using ModernKeePass.Application.Group.Commands.RemoveEntry;
using ModernKeePass.Application.Group.Queries.GetGroup;
using ModernKeePass.Application.Security.Commands.GeneratePassword;
using ModernKeePass.Application.Security.Queries.EstimatePasswordComplexity;
using ModernKeePass.Common;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.AOP;

namespace ModernKeePass.ViewModels
{
    public class EntryDetailVm : NotifyPropertyChangedBase, IVmEntity
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

        public IEnumerable<GroupVm> BreadCrumb => new List<GroupVm> { _parent };

        /// <summary>
        /// Determines if the Entry is current or from history
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public double PasswordLength
        {
            get { return _passwordLength; }
            set { SetProperty(ref _passwordLength, value); }
        }

        public string Title
        {
            get { return _entry.Title; }
            set
            {
                SetFieldValue(nameof(Title), value).Wait();
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
                SetFieldValue(nameof(Password), value).Wait();
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
                SetFieldValue(nameof(Url), value).Wait();
                _entry.Url = new Uri(value);
            }
        }

        public string Notes
        {
            get { return _entry.Notes; }
            set
            {
                SetFieldValue(nameof(Notes), value).Wait();
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
                SetFieldValue(nameof(Icon), value).Wait();
                _entry.Icon = (Icon)value;
            }
        }

        public DateTimeOffset ExpiryDate
        {
            get { return _entry.ExpirationDate; }
            set
            {
                if (!HasExpirationDate) return;

                SetFieldValue("ExpirationDate", value).Wait();
                _entry.ExpirationDate = value.Date;
            }
        }

        public TimeSpan ExpiryTime
        {
            get { return _entry.ExpirationDate.TimeOfDay; }
            set
            {
                if (!HasExpirationDate) return;

                SetFieldValue("ExpirationDate", value).Wait();
                _entry.ExpirationDate = _entry.ExpirationDate.Date.Add(value);
            }
        }
        
        public bool HasExpirationDate
        {
            get { return _entry.HasExpirationDate; }
            set
            {
                SetFieldValue(nameof(HasExpirationDate), value).Wait();
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
                    SetFieldValue(nameof(BackgroundColor), value).Wait();
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
                    SetFieldValue(nameof(ForegroundColor), value).Wait();
                    _entry.ForegroundColor = (Color)value;
                }
            }
        }
        public IEnumerable<EntryVm> History { get; }

        public bool IsEditMode
        {
            get { return IsSelected && _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
        }
        
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }
        
        public bool IsRevealPassword
        {
            get { return _isRevealPassword; }
            set { SetProperty(ref _isRevealPassword, value); }
        }

        public ICommand SaveCommand { get; }
        public ICommand GeneratePasswordCommand { get; }
        public ICommand MoveCommand { get; }

        private DatabaseVm Database => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();

        private readonly IMediator _mediator;
        private readonly GroupVm _parent;
        private EntryVm _entry;
        private bool _isEditMode;
        private bool _isRevealPassword;
        private double _passwordLength = 25;
        private bool _isVisible = true;
        private bool _isSelected;

        public EntryDetailVm() { }
        
        internal EntryDetailVm(string entryId, bool isNewEntry = false) : this(entryId, App.Services.GetRequiredService<IMediator>(), isNewEntry) { }

        public EntryDetailVm(string entryId, IMediator mediator, bool isNewEntry = false)
        {
            _mediator = mediator;
            _entry = _mediator.Send(new GetEntryQuery {Id = entryId}).GetAwaiter().GetResult();
            _parent = _mediator.Send(new GetGroupQuery {Id = _entry.ParentGroupId}).GetAwaiter().GetResult();
            History = _entry.History;
            _isEditMode = isNewEntry;
            if (isNewEntry) GeneratePassword().GetAwaiter().GetResult();
            IsSelected = true;

            SaveCommand = new RelayCommand(async () => await SaveChanges(), () => Database.IsDirty);
            GeneratePasswordCommand = new RelayCommand(async () => await GeneratePassword());
            MoveCommand = new RelayCommand(async () => await Move(_parent), () => _parent != null);
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
            await _mediator.Send(new DeleteEntryCommand {EntryId = _entry.Id, ParentGroupId = _entry.ParentGroupId, RecycleBinName = recycleBinTitle});
        }
        
        public async Task Move(GroupVm destination)
        {
            await _mediator.Send(new AddEntryCommand { ParentGroup = destination, Entry = _entry });
            await _mediator.Send(new RemoveEntryCommand { ParentGroup = _parent, Entry = _entry });
        }
        
        public async Task SetFieldValue(string fieldName, object value)
        {
            await _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = fieldName, FieldValue = value });
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        internal void SetEntry(EntryVm entry, int index)
        {
            _entry = entry;
            IsSelected = index == 0;
        }

        private async Task SaveChanges()
        {
            await _mediator.Send(new SaveDatabaseCommand());
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }
}
