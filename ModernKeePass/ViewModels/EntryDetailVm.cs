using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Commands.AddHistory;
using ModernKeePass.Application.Entry.Commands.DeleteHistory;
using ModernKeePass.Application.Entry.Commands.RestoreHistory;
using ModernKeePass.Application.Entry.Commands.SetFieldValue;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Entry.Queries.GetEntry;
using ModernKeePass.Application.Group.Commands.AddEntry;
using ModernKeePass.Application.Group.Commands.DeleteEntry;
using ModernKeePass.Application.Group.Commands.RemoveEntry;
using ModernKeePass.Application.Group.Queries.GetGroup;
using ModernKeePass.Application.Security.Commands.GeneratePassword;
using ModernKeePass.Application.Security.Queries.EstimatePasswordComplexity;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Extensions;

namespace ModernKeePass.ViewModels
{
    public class EntryDetailVm : ObservableObject
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
        public string Id => SelectedItem.Id;

        public bool IsRecycleOnDelete
        {
            get
            {
                var database = Database;
                return database.IsRecycleBinEnabled && _parent.Id != database.RecycleBinId;
            }
        } 

        public IEnumerable<GroupVm> BreadCrumb => new List<GroupVm> { _parent };
        public ObservableCollection<EntryVm> History { get; }

        /// <summary>
        /// Determines if the Entry is current or from history
        /// </summary>
        public bool IsCurrentEntry => SelectedIndex == 0;

        public EntryVm SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                Set(() => SelectedItem, ref _selectedItem, value);
                if (value != null) RaisePropertyChanged();
            }
        }
        public int SelectedIndex    
        {
            get { return _selectedIndex; }
            set
            {
                Set(() => SelectedIndex, ref _selectedIndex, value);
                RaisePropertyChanged(nameof(IsCurrentEntry));
            }
        }

        public double PasswordLength
        {
            get { return _passwordLength; }
            set { Set(() => PasswordLength, ref _passwordLength, value); }
        }

        public string Title
        {
            get { return SelectedItem.Title; }
            set
            {
                SelectedItem.Title = value;
                SetFieldValue(nameof(Title), value).Wait();
            }
        }

        public string UserName
        {
            get { return SelectedItem.Username; }
            set { SelectedItem.Username = value; }
        }

        public string Password
        {
            get { return SelectedItem.Password; }
            set
            {
                SelectedItem.Password = value;
                SetFieldValue(nameof(Password), value).Wait();
                RaisePropertyChanged(nameof(Password));
                RaisePropertyChanged(nameof(PasswordComplexityIndicator));
            }
        }
        
        public string Url
        {
            get { return SelectedItem.Url; }
            set
            {
                SelectedItem.Url = value;
                SetFieldValue(nameof(Url), value).Wait();
            }
        }

        public string Notes
        {
            get { return SelectedItem.Notes; }
            set
            {
                SelectedItem.Notes = value;
                SetFieldValue(nameof(Notes), value).Wait();
            }
        }

        public Symbol Icon
        {
            get { return (Symbol)Enum.Parse(typeof(Symbol), SelectedItem.Icon.ToString()); }
            set
            {
                SelectedItem.Icon = (Icon)Enum.Parse(typeof(Icon), value.ToString());
                SetFieldValue(nameof(Icon), SelectedItem.Icon).Wait();
            }
        }

        public DateTimeOffset ExpiryDate
        {
            get { return SelectedItem.ExpirationDate; }
            set
            {
                if (!HasExpirationDate) return;

                SelectedItem.ExpirationDate = value.Date;
                SetFieldValue("ExpirationDate", SelectedItem.ExpirationDate).Wait();
            }
        }

        public TimeSpan ExpiryTime
        {
            get { return SelectedItem.ExpirationDate.TimeOfDay; }
            set
            {
                if (!HasExpirationDate) return;

                SelectedItem.ExpirationDate = SelectedItem.ExpirationDate.Date.Add(value);
                SetFieldValue("ExpirationDate", SelectedItem.ExpirationDate).Wait();
            }
        }
        
        public bool HasExpirationDate
        {
            get { return SelectedItem.HasExpirationDate; }
            set
            {
                SelectedItem.HasExpirationDate = value;
                SetFieldValue(nameof(HasExpirationDate), value).Wait();
                RaisePropertyChanged(nameof(HasExpirationDate));
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get { return SelectedItem?.BackgroundColor.ToSolidColorBrush(); }
            set
            {
                SelectedItem.BackgroundColor = value.ToColor();
                SetFieldValue(nameof(BackgroundColor), SelectedItem.BackgroundColor).Wait();
            }
        }

        public SolidColorBrush ForegroundColor
        {
            get { return SelectedItem?.ForegroundColor.ToSolidColorBrush(); }
            set
            {
                SelectedItem.ForegroundColor = value.ToColor();
                SetFieldValue(nameof(ForegroundColor), SelectedItem.ForegroundColor).Wait();
            }
        }


        public bool IsEditMode
        {
            get { return IsCurrentEntry && _isEditMode; }
            set { Set(() => IsEditMode, ref _isEditMode, value); }
        }
        
        public bool IsRevealPassword
        {
            get { return _isRevealPassword; }
            set { Set(() => IsRevealPassword, ref _isRevealPassword, value); }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand GeneratePasswordCommand { get; }
        public RelayCommand MoveCommand { get; }
        public RelayCommand RestoreCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand GoBackCommand { get; }

        private DatabaseVm Database => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();

        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        private readonly IResourceProxy _resource;
        private readonly IDialogService _dialog;
        private readonly INotificationService _notification;
        private readonly GroupVm _parent;
        private EntryVm _selectedItem;
        private int _selectedIndex;
        private bool _isEditMode;
        private bool _isRevealPassword;
        private double _passwordLength = 25;
        private bool _isDirty;

        public EntryDetailVm() { }
        
        internal EntryDetailVm(string entryId) : this(entryId, 
            App.Services.GetRequiredService<IMediator>(), 
            App.Services.GetRequiredService<INavigationService>(), 
            App.Services.GetRequiredService<IResourceProxy>(), 
            App.Services.GetRequiredService<IDialogService>(), 
            App.Services.GetRequiredService<INotificationService>()) { }

        public EntryDetailVm(string entryId, IMediator mediator, INavigationService navigation, IResourceProxy resource, IDialogService dialog, INotificationService notification)
        {
            _mediator = mediator;
            _navigation = navigation;
            _resource = resource;
            _dialog = dialog;
            _notification = notification;
            SelectedItem = _mediator.Send(new GetEntryQuery { Id = entryId }).GetAwaiter().GetResult();
            _parent = _mediator.Send(new GetGroupQuery { Id = SelectedItem.ParentGroupId }).GetAwaiter().GetResult();
            History = new ObservableCollection<EntryVm> { SelectedItem };
            foreach (var entry in SelectedItem.History.Skip(1))
            {
                History.Add(entry);
            }
            SelectedIndex = 0;

            SaveCommand = new RelayCommand(async () => await SaveChanges(), () => Database.IsDirty);
            GeneratePasswordCommand = new RelayCommand(async () => await GeneratePassword());
            MoveCommand = new RelayCommand(async () => await Move(_parent), () => _parent != null);
            RestoreCommand = new RelayCommand(async () => await RestoreHistory());
            DeleteCommand = new RelayCommand(async () => await AskForDelete());
            GoBackCommand = new RelayCommand(() => _navigation.GoBack());
        }

        private async Task AskForDelete()
        {
            if (IsCurrentEntry)
            {
                if (IsRecycleOnDelete)
                {
                    await Delete();
                    _notification.Show(_resource.GetResourceValue("EntryRecyclingConfirmation"), _resource.GetResourceValue("EntryRecycled"));
                }
                else
                {
                    await _dialog.ShowMessage(_resource.GetResourceValue("EntryDeletingConfirmation"),
                        _resource.GetResourceValue("EntityDeleteTitle"),
                        _resource.GetResourceValue("EntityDeleteActionButton"),
                        _resource.GetResourceValue("EntityDeleteCancelButton"),
                        async isOk =>
                        {
                            if (isOk) await Delete();
                        });
                }
            }
            else
            {
                await _dialog.ShowMessage(_resource.GetResourceValue("HistoryDeleteDescription"), _resource.GetResourceValue("HistoryDeleteTitle"),
                    _resource.GetResourceValue("EntityDeleteActionButton"),
                    _resource.GetResourceValue("EntityDeleteCancelButton"), async isOk =>
                    {
                        if (!isOk) return;
                        await _mediator.Send(new DeleteHistoryCommand { Entry = History[0], HistoryIndex = History.Count - SelectedIndex - 1 });
                        History.RemoveAt(SelectedIndex);
                        SelectedIndex = 0;
                        SaveCommand.RaiseCanExecuteChanged();
                    });
            }
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
            RaisePropertyChanged(nameof(IsRevealPasswordEnabled));
        }
        
        public async Task Move(GroupVm destination)
        {
            await _mediator.Send(new AddEntryCommand { ParentGroup = destination, Entry = SelectedItem });
            await _mediator.Send(new RemoveEntryCommand { ParentGroup = _parent, Entry = SelectedItem });
        }
        
        public async Task SetFieldValue(string fieldName, object value)
        {
            await _mediator.Send(new SetFieldValueCommand { EntryId = Id, FieldName = fieldName, FieldValue = value });
            SaveCommand.RaiseCanExecuteChanged();
            _isDirty = true;
        }

        public async Task AddHistory()
        {
            if (_isDirty) await _mediator.Send(new AddHistoryCommand { Entry = History[0] });
        }
        
        private async Task RestoreHistory()
        {
            await _mediator.Send(new RestoreHistoryCommand { Entry = History[0], HistoryIndex = History.Count - SelectedIndex - 1 });
            History.Insert(0, SelectedItem);
            SelectedIndex = 0;
            SaveCommand.RaiseCanExecuteChanged();
        }
        
        private async Task SaveChanges()
        {
            await AddHistory();
            await _mediator.Send(new SaveDatabaseCommand());
            SaveCommand.RaiseCanExecuteChanged();
            _isDirty = false;
        }

        private async Task Delete()
        {
            await _mediator.Send(new DeleteEntryCommand
            {
                EntryId = Id,
                ParentGroupId = SelectedItem.ParentGroupId,
                RecycleBinName = _resource.GetResourceValue("RecycleBinTitle")
            });
            _navigation.GoBack();
        }
    }
}
