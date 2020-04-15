using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Commands.AddHistory;
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
using ModernKeePass.Extensions;

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
                _entry.Title = value;
                SetFieldValue(nameof(Title), value).Wait();
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
                _entry.Password = value;
                SetFieldValue(nameof(Password), value).Wait();
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(PasswordComplexityIndicator));
            }
        }
        
        public string Url
        {
            get { return _entry.Url?.ToString(); }
            set
            {
                _entry.Url = new Uri(value);
                SetFieldValue(nameof(Url), value).Wait();
            }
        }

        public string Notes
        {
            get { return _entry.Notes; }
            set
            {
                _entry.Notes = value;
                SetFieldValue(nameof(Notes), value).Wait();
            }
        }

        public Symbol Icon
        {
            get { return (Symbol)Enum.Parse(typeof(Symbol), _entry.Icon.ToString()); }
            set
            {
                _entry.Icon = (Icon)Enum.Parse(typeof(Icon), value.ToString());
                SetFieldValue(nameof(Icon), _entry.Icon).Wait();
            }
        }

        public DateTimeOffset ExpiryDate
        {
            get { return _entry.ExpirationDate; }
            set
            {
                if (!HasExpirationDate) return;

                _entry.ExpirationDate = value.Date;
                SetFieldValue("ExpirationDate", _entry.ExpirationDate).Wait();
            }
        }

        public TimeSpan ExpiryTime
        {
            get { return _entry.ExpirationDate.TimeOfDay; }
            set
            {
                if (!HasExpirationDate) return;

                _entry.ExpirationDate = _entry.ExpirationDate.Date.Add(value);
                SetFieldValue("ExpirationDate", _entry.ExpirationDate).Wait();
            }
        }
        
        public bool HasExpirationDate
        {
            get { return _entry.HasExpirationDate; }
            set
            {
                _entry.HasExpirationDate = value;
                SetFieldValue(nameof(HasExpirationDate), value).Wait();
                OnPropertyChanged(nameof(HasExpirationDate));
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get { return _entry?.BackgroundColor.ToSolidColorBrush(); }
            set
            {
                _entry.BackgroundColor = value.ToColor();
                SetFieldValue(nameof(BackgroundColor), _entry.BackgroundColor).Wait();
            }
        }

        public SolidColorBrush ForegroundColor
        {
            get { return _entry?.ForegroundColor.ToSolidColorBrush(); }
            set
            {
                _entry.ForegroundColor = value.ToColor();
                SetFieldValue(nameof(ForegroundColor), _entry.ForegroundColor).Wait();
            }
        }
        public IEnumerable<EntryVm> History { get; }

        public bool IsEditMode
        {
            get { return IsSelected && _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
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
        private bool _isSelected;

        public EntryDetailVm() { }
        
        internal EntryDetailVm(string entryId) : this(entryId, App.Services.GetRequiredService<IMediator>()) { }

        public EntryDetailVm(string entryId, IMediator mediator)
        {
            _mediator = mediator;
            _entry = _mediator.Send(new GetEntryQuery {Id = entryId}).GetAwaiter().GetResult();
            _parent = _mediator.Send(new GetGroupQuery {Id = _entry.ParentGroupId}).GetAwaiter().GetResult();
            History = _entry.History;
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
            await _mediator.Send(new DeleteEntryCommand {EntryId = Id, ParentGroupId = _entry.ParentGroupId, RecycleBinName = recycleBinTitle});
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
            _entry.IsDirty = true;
        }

        public async Task AddHistory()
        {
            if (_entry.IsDirty) await _mediator.Send(new AddHistoryCommand {EntryId = Id});
        }

        internal void SetEntry(EntryVm entry, int index)
        {
            _entry = entry;
            IsSelected = index == 0;
            OnPropertyChanged();
        }

        private async Task SaveChanges()
        {
            await AddHistory();
            await _mediator.Send(new SaveDatabaseCommand());
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            _entry.IsDirty = false;
        }

    }
}
