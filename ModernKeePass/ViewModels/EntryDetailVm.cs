using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Commands.AddAttachment;
using ModernKeePass.Application.Entry.Commands.AddHistory;
using ModernKeePass.Application.Entry.Commands.DeleteAttachment;
using ModernKeePass.Application.Entry.Commands.DeleteField;
using ModernKeePass.Application.Entry.Commands.DeleteHistory;
using ModernKeePass.Application.Entry.Commands.RestoreHistory;
using ModernKeePass.Application.Entry.Commands.UpsertField;
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
using ModernKeePass.Common;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;
using ModernKeePass.Extensions;
using ModernKeePass.Models;

namespace ModernKeePass.ViewModels
{
    public class EntryDetailVm : ViewModelBase
    {
        public bool HasExpired => HasExpirationDate && ExpiryDate < DateTime.Now;
        public double PasswordComplexityIndicator => _mediator.Send(new EstimatePasswordComplexityQuery {Password = Password}).GetAwaiter().GetResult();
        public double PasswordLength
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.PasswordLength, 25); }
            set
            {
                _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.PasswordLength, value);
                RaisePropertyChanged(nameof(PasswordLength));
            }
        }
        public bool UpperCasePatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.UpperCasePattern, true); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.UpperCasePattern, value); }
        }
        public bool LowerCasePatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.LowerCasePattern, true); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.LowerCasePattern, value); }
        }
        public bool DigitsPatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.DigitsPattern, true); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.DigitsPattern, value); }
        }
        public bool MinusPatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.MinusPattern, false); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.MinusPattern, value); }
        }
        public bool UnderscorePatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.UnderscorePattern, false); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.UnderscorePattern, value); }
        }
        public bool SpacePatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.SpacePattern, false); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.SpacePattern, value); }
        }
        public bool SpecialPatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.SpecialPattern, true); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.SpecialPattern, value); }
        }
        public bool BracketsPatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.BracketsPattern, false); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.BracketsPattern, value); }
        }
        public string CustomChars { get; set; } = string.Empty;

        public string Id => SelectedItem.Id;

        public string ParentGroupName => _parent.Title;

        public bool IsRecycleOnDelete
        {
            get
            {
                var database = Database;
                return database.IsRecycleBinEnabled && _parent.Id != database.RecycleBinId;
            }
        } 
        
        public ObservableCollection<EntryVm> History { get; private set; }
        public ObservableCollection<FieldVm> AdditionalFields { get; private set; }
        public ObservableCollection<Attachment> Attachments { get; private set; }

        /// <summary>
        /// Determines if the Entry is current or from history
        /// </summary>
        public bool IsCurrentEntry => SelectedIndex == 0;

        public EntryVm SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                Set(() => SelectedItem, ref _selectedItem, value, true);
                if (value != null)
                {
                    AdditionalFields = new ObservableCollection<FieldVm>(SelectedItem.AdditionalFields);

                    Attachments = new ObservableCollection<Attachment>(SelectedItem.Attachments.Select(f => new Attachment
                    {
                        Name = f.Key,
                        Content = f.Value
                    }));
                    Attachments.CollectionChanged += (sender, args) =>
                    {
                        UpdateDirtyStatus(true);
                    };
                    RaisePropertyChanged(string.Empty);
                }
            }
        }
        
        public int SelectedIndex    
        {
            get { return _selectedIndex; }
            set
            {
                Set(() => SelectedIndex, ref _selectedIndex, value);
                RaisePropertyChanged(nameof(IsCurrentEntry));
                AddAttachmentCommand.RaiseCanExecuteChanged();
            }
        }

        public int AdditionalFieldSelectedIndex
        {
            get { return _additionalFieldSelectedIndex; }
            set
            {
                Set(() => AdditionalFieldSelectedIndex, ref _additionalFieldSelectedIndex, value);
                DeleteAdditionalField.RaiseCanExecuteChanged();
                if (value != -1)
                {
                    var additionalField = AdditionalFields[value];
                    Set(nameof(AdditionalFieldName), ref _additionalFieldName, additionalField.Name);
                    Set(nameof(AdditionalFieldValue), ref _additionalFieldValue, additionalField.IsProtected ? _cryptography.UnProtect(additionalField.Value).GetAwaiter().GetResult() : additionalField.Value);
                    Set(nameof(AdditionalFieldIsProtected), ref _additionalFieldIsProtected, additionalField.IsProtected);
                }
            }
        }
        
        public string Title
        {
            get { return SelectedItem.Title.Value; }
            set
            {
                SelectedItem.Title.Value = value;
                SetFieldValue(nameof(Title), value, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public string UserName
        {
            get { return SelectedItem.Username.Value; }
            set
            {
                SelectedItem.Username.Value = value;
                SetFieldValue(nameof(UserName), value, false).ConfigureAwait(false).GetAwaiter();
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get { return _cryptography.UnProtect(SelectedItem.Password.Value).GetAwaiter().GetResult(); }
            set
            {
                // TODO: cleanup this
                var protectedPassword = _cryptography.Protect(value).ConfigureAwait(false).GetAwaiter().GetResult();
                SelectedItem.Password.Value = protectedPassword;
                SetFieldValue(nameof(Password), value, true).ConfigureAwait(false).GetAwaiter();
                RaisePropertyChanged(nameof(Password));
                RaisePropertyChanged(nameof(PasswordComplexityIndicator));
            }
        }
        
        public string Url
        {
            get { return SelectedItem.Url.Value; }
            set
            {
                SelectedItem.Url.Value = value;
                SetFieldValue(nameof(Url), value, false).ConfigureAwait(false).GetAwaiter();
                RaisePropertyChanged(nameof(Url));
            }
        }

        public string Notes
        {
            get { return SelectedItem.Notes.Value; }
            set
            {
                SelectedItem.Notes.Value = value;
                SetFieldValue(nameof(Notes), value, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public Symbol Icon
        {
            get { return (Symbol)Enum.Parse(typeof(Symbol), SelectedItem.Icon.ToString()); }
            set
            {
                SelectedItem.Icon = (Icon)Enum.Parse(typeof(Icon), value.ToString());
                SetFieldValue(nameof(Icon), SelectedItem.Icon, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public DateTimeOffset ExpiryDate
        {
            get { return SelectedItem.ExpirationDate; }
            set
            {
                if (!HasExpirationDate) return;

                SelectedItem.ExpirationDate = value.Date;
                SetFieldValue("ExpirationDate", SelectedItem.ExpirationDate, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public TimeSpan ExpiryTime
        {
            get { return SelectedItem.ExpirationDate.TimeOfDay; }
            set
            {
                if (!HasExpirationDate) return;

                SelectedItem.ExpirationDate = SelectedItem.ExpirationDate.Date.Add(value);
                SetFieldValue("ExpirationDate", SelectedItem.ExpirationDate, false).ConfigureAwait(false).GetAwaiter();
            }
        }
        
        public bool HasExpirationDate
        {
            get { return SelectedItem.HasExpirationDate; }
            set
            {
                SelectedItem.HasExpirationDate = value;
                SetFieldValue(nameof(HasExpirationDate), value, false).ConfigureAwait(false).GetAwaiter();
                RaisePropertyChanged(nameof(HasExpirationDate));
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get { return SelectedItem?.BackgroundColor.ToSolidColorBrush(); }
            set
            {
                SelectedItem.BackgroundColor = value.ToColor();
                SetFieldValue(nameof(BackgroundColor), SelectedItem.BackgroundColor, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public SolidColorBrush ForegroundColor
        {
            get { return SelectedItem?.ForegroundColor.ToSolidColorBrush(); }
            set
            {
                SelectedItem.ForegroundColor = value.ToColor();
                SetFieldValue(nameof(ForegroundColor), SelectedItem.ForegroundColor, false).ConfigureAwait(false).GetAwaiter();
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

        private string _additionalFieldName;
        private string _additionalFieldValue;
        private bool _additionalFieldIsProtected;

        public string AdditionalFieldName
        {
            get { return _additionalFieldName; }
            set
            {
                var newName = EntryFieldName.StandardFieldNames.Contains(value) ? $"{value}_1" : value;
                UpdateFieldName(_additionalFieldName, newName, AdditionalFieldValue, AdditionalFieldIsProtected).ConfigureAwait(false).GetAwaiter();
            }
        }

        public string AdditionalFieldValue
        {
            get
            {
                return  _additionalFieldValue;
            }
            set
            {
                SetFieldValue(AdditionalFieldName, value, AdditionalFieldIsProtected).ConfigureAwait(false).GetAwaiter();
            }
        }

        public bool AdditionalFieldIsProtected
        {
            get { return _additionalFieldIsProtected; }
            set
            {
                SetFieldValue(AdditionalFieldName, AdditionalFieldValue, value).ConfigureAwait(false).GetAwaiter();
            }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand GeneratePasswordCommand { get; }
        public RelayCommand<string> MoveCommand { get; }
        public RelayCommand RestoreCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand GoBackCommand { get; }
        public RelayCommand GoToParentCommand { get; set; }
        public RelayCommand AddAdditionalField { get; set; }
        public RelayCommand<FieldVm> DeleteAdditionalField { get; set; }
        public RelayCommand<Attachment> OpenAttachmentCommand { get; set; }
        public RelayCommand AddAttachmentCommand { get; set; }
        public RelayCommand<Attachment> DeleteAttachmentCommand { get; set; }

        private DatabaseVm Database => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
        
        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        private readonly IResourceProxy _resource;
        private readonly IDialogService _dialog;
        private readonly INotificationService _notification;
        private readonly IFileProxy _file;
        private readonly ISettingsProxy _settings;
        private readonly ICryptographyClient _cryptography;
        private GroupVm _parent;
        private EntryVm _selectedItem;
        private int _selectedIndex;
        private int _additionalFieldSelectedIndex = -1;
        private bool _isEditMode;
        private bool _isRevealPassword;
        private bool _isDirty;

        public EntryDetailVm(IMediator mediator, INavigationService navigation, IResourceProxy resource, IDialogService dialog, INotificationService notification, IFileProxy file, ISettingsProxy settings, ICryptographyClient cryptography)
        {
            _mediator = mediator;
            _navigation = navigation;
            _resource = resource;
            _dialog = dialog;
            _notification = notification;
            _file = file;
            _settings = settings;
            _cryptography = cryptography;

            SaveCommand = new RelayCommand(async () => await SaveChanges(), () => Database.IsDirty);
            GeneratePasswordCommand = new RelayCommand(async () => await GeneratePassword());
            MoveCommand = new RelayCommand<string>(async destination => await Move(destination), destination => _parent != null && !string.IsNullOrEmpty(destination) && destination != _parent.Id);
            RestoreCommand = new RelayCommand(async () => await RestoreHistory());
            DeleteCommand = new RelayCommand(async () => await AskForDelete());
            GoBackCommand = new RelayCommand(() => _navigation.GoBack());
            GoToParentCommand = new RelayCommand(() => GoToGroup(_parent.Id));
            AddAdditionalField = new RelayCommand(AddField, () => IsCurrentEntry);
            DeleteAdditionalField = new RelayCommand<FieldVm>(async field => await DeleteField(field), field => field != null && IsCurrentEntry);
            OpenAttachmentCommand = new RelayCommand<Attachment>(async attachment => await OpenAttachment(attachment));
            AddAttachmentCommand = new RelayCommand(async () => await AddAttachment(), () => IsCurrentEntry);
            DeleteAttachmentCommand = new RelayCommand<Attachment>(async attachment => await DeleteAttachment(attachment), _ => IsCurrentEntry);

            MessengerInstance.Register<DatabaseSavedMessage>(this, _ => SaveCommand.RaiseCanExecuteChanged());
            MessengerInstance.Register<EntryFieldValueChangedMessage>(this, async message => await SetFieldValue(message.FieldName, message.FieldValue, message.IsProtected));
            MessengerInstance.Register<EntryFieldNameChangedMessage>(this, async message => await UpdateFieldName(message.OldName, message.NewName, message.Value, message.IsProtected));
        }

        public async Task Initialize(string entryId)
        {
            SelectedIndex = 0;
            SelectedItem = await _mediator.Send(new GetEntryQuery { Id = entryId });
            _parent = await _mediator.Send(new GetGroupQuery { Id = SelectedItem.ParentGroupId });
            History = new ObservableCollection<EntryVm> { SelectedItem };
            foreach (var entry in SelectedItem.History.Skip(1))
            {
                History.Add(entry);
            }
            History.CollectionChanged += (sender, args) =>
            {
                SelectedIndex = 0;
                SaveCommand.RaiseCanExecuteChanged();
            };
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
        }

        public async Task AddHistory()
        {
            if (_isDirty) await _mediator.Send(new AddHistoryCommand { Entry = History[0] });
        }
        
        public void GoToGroup(string groupId)
        {
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = groupId });
        }
        
        private async Task Move(string destination)
        {
            await _mediator.Send(new AddEntryCommand { ParentGroupId = destination, EntryId = Id });
            await _mediator.Send(new RemoveEntryCommand { ParentGroupId = _parent.Id, EntryId = Id });
            GoToGroup(destination);
        }
        
        private async Task SetFieldValue(string fieldName, object value, bool isProtected)
        {
            var protectedValue = isProtected ? await _cryptography.Protect(value?.ToString()) : value;
            await _mediator.Send(new UpsertFieldCommand { EntryId = Id, FieldName = fieldName, FieldValue = protectedValue, IsProtected = isProtected});
            UpdateDirtyStatus(true);
        }

        private async Task UpdateFieldName(string oldName, string newName, string value, bool isProtected)
        {
            if (!string.IsNullOrEmpty(oldName)) await _mediator.Send(new DeleteFieldCommand { EntryId = Id, FieldName = oldName });
            await SetFieldValue(newName, value, isProtected);
        }
        
        private void AddField()
        {
            AdditionalFields.Add(new FieldVm());
            AdditionalFieldSelectedIndex = AdditionalFields.Count - 1;
        }

        private async Task DeleteField(FieldVm field)
        {
            AdditionalFields.Remove(field);
            if (!string.IsNullOrEmpty(field.Name))
            {
                await _mediator.Send(new DeleteFieldCommand {EntryId = Id, FieldName = field.Name});
                UpdateDirtyStatus(true);
            }
        }

        private async Task AskForDelete()
        {
            if (IsCurrentEntry)
            {
                if (IsRecycleOnDelete)
                {
                    await Delete();
                    _notification.Show(_resource.GetResourceValue("EntityDeleting"), string.Format(_resource.GetResourceValue("EntryRecycled"), Title));
                }
                else
                {
                    await _dialog.ShowMessage(
                        string.Format(_resource.GetResourceValue("EntryDeletingConfirmation"), Title),
                        _resource.GetResourceValue("EntityDeleting"),
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
                await _dialog.ShowMessage(_resource.GetResourceValue("HistoryDeleteDescription"), 
                    _resource.GetResourceValue("HistoryDeleteTitle"),
                    _resource.GetResourceValue("EntityDeleteActionButton"),
                    _resource.GetResourceValue("EntityDeleteCancelButton"), async isOk =>
                    {
                        if (!isOk) return;
                        await _mediator.Send(new DeleteHistoryCommand { Entry = History[0], HistoryIndex = History.Count - SelectedIndex - 1 });
                        History.RemoveAt(SelectedIndex);
                    });
            }
        }

        private async Task RestoreHistory()
        {
            await _mediator.Send(new RestoreHistoryCommand { Entry = History[0], HistoryIndex = History.Count - SelectedIndex - 1 });
            History.Insert(0, SelectedItem);
        }
        
        private async Task SaveChanges()
        {
            await AddHistory();
            try
            {
                await _mediator.Send(new SaveDatabaseCommand());
            }
            catch (SaveException e)
            {
                MessengerInstance.Send(new SaveErrorMessage { Message = e.Message });
            }
            UpdateDirtyStatus(false);
        }

        private async Task Delete()
        {
            await _mediator.Send(new DeleteEntryCommand
            {
                EntryId = Id,
                ParentGroupId = SelectedItem.ParentGroupId,
                RecycleBinName = _resource.GetResourceValue("RecycleBinTitle")
            });
            _isDirty = false;
            _navigation.GoBack();
        }

        private async Task OpenAttachment(Attachment attachment)
        {
            var extensionIndex = attachment.Name.LastIndexOf('.');
            var fileInfo = await _file.CreateFile(attachment.Name,
                attachment.Name.Substring(extensionIndex, attachment.Name.Length - extensionIndex), 
                string.Empty,
                false);
            if (fileInfo == null) return;
            await _file.WriteBinaryContentsToFile(fileInfo.Id, attachment.Content);
        }

        private async Task AddAttachment()
        {
            var fileInfo = await _file.OpenFile(string.Empty, Domain.Common.Constants.Extensions.Any, false);
            if (fileInfo == null) return;
            var contents = await _file.ReadBinaryFile(fileInfo.Id);
            await _mediator.Send(new AddAttachmentCommand { Entry = SelectedItem, AttachmentName = fileInfo.Name, AttachmentContent = contents });
            Attachments.Add(new Attachment { Name = fileInfo.Name, Content = contents });
        }

        private async Task DeleteAttachment(Attachment attachment)
        {
            await _mediator.Send(new DeleteAttachmentCommand { Entry = SelectedItem, AttachmentName = attachment.Name });
            Attachments.Remove(attachment);
        }

        private void UpdateDirtyStatus(bool isDirty)
        {
            SaveCommand.RaiseCanExecuteChanged();
            _isDirty = isDirty;
        }
    }
}
