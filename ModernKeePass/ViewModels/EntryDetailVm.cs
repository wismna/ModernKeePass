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
using ModernKeePass.Domain.Enums;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Common;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Extensions;
using ModernKeePass.Models;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    public class EntryDetailVm : ViewModelBase
    {
        public bool HasExpired => HasExpirationDate && ExpiryDate < _dateTime.Now;
        
        public string Id => _current.Id;
        
        public bool IsRecycleOnDelete
        {
            get
            {
                var database = Database;
                return database.IsRecycleBinEnabled && _parent.Id != database.RecycleBinId;
            }
        } 
        
        public ObservableCollection<IEntityVm> History { get; private set; }
        public ObservableCollection<EntryFieldVm> AdditionalFields { get; private set; }
        public ObservableCollection<Attachment> Attachments { get; private set; }

        /// <summary>
        /// Determines if the Entry is current or from history
        /// </summary>
        public bool IsCurrentEntry => SelectedIndex == 0;
        
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
            }
        }
        
        public string Title
        {
            get { return _current.Title.Value; }
            set
            {
                _current.Title.Value = value;
                SetFieldValue(_current.Title.Name, value, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public string UserName
        {
            get { return _current.Username.Value; }
            set
            {
                _current.Username.Value = value;
                SetFieldValue(_current.Username.Name, value, false).ConfigureAwait(false).GetAwaiter();
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get { return _cryptography.UnProtect(_current.Password.Value).GetAwaiter().GetResult(); }
            set
            {
                var protectedPassword = _cryptography.Protect(value).ConfigureAwait(false).GetAwaiter().GetResult();
                _current.Password.Value = protectedPassword;
                SetFieldValue(_current.Password.Name, protectedPassword, true).ConfigureAwait(false).GetAwaiter();

                RaisePropertyChanged(nameof(Password));
            }
        }
        
        public string Url
        {
            get { return _current.Url.Value; }
            set
            {
                _current.Url.Value = value;
                SetFieldValue(_current.Url.Name, value, false).ConfigureAwait(false).GetAwaiter();
                RaisePropertyChanged(nameof(Url));
            }
        }

        public string Notes
        {
            get { return _current.Notes.Value; }
            set
            {
                _current.Notes.Value = value;
                SetFieldValue(_current.Notes.Name, value, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public Symbol Icon
        {
            get { return (Symbol)Enum.Parse(typeof(Symbol), _current.Icon.ToString()); }
            set
            {
                _current.Icon = (Icon)Enum.Parse(typeof(Icon), value.ToString());
                SetFieldValue(EntryFieldName.Icon, _current.Icon, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public DateTimeOffset ExpiryDate
        {
            get { return _current.ExpirationDate; }
            set
            {
                if (!HasExpirationDate) return;

                _current.ExpirationDate = value.Date;
                SetFieldValue(EntryFieldName.ExpirationDate, _current.ExpirationDate, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public TimeSpan ExpiryTime
        {
            get { return _current.ExpirationDate.TimeOfDay; }
            set
            {
                if (!HasExpirationDate) return;

                _current.ExpirationDate = _current.ExpirationDate.Date.Add(value);
                SetFieldValue(EntryFieldName.ExpirationDate, _current.ExpirationDate, false).ConfigureAwait(false).GetAwaiter();
            }
        }
        
        public bool HasExpirationDate
        {
            get { return _current.HasExpirationDate; }
            set
            {
                _current.HasExpirationDate = value;
                SetFieldValue(EntryFieldName.HasExpirationDate, value, false).ConfigureAwait(false).GetAwaiter();
                RaisePropertyChanged(nameof(HasExpirationDate));
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get { return _current?.BackgroundColor.ToSolidColorBrush(); }
            set
            {
                _current.BackgroundColor = value.ToColor();
                SetFieldValue(EntryFieldName.BackgroundColor, _current.BackgroundColor, false).ConfigureAwait(false).GetAwaiter();
            }
        }

        public SolidColorBrush ForegroundColor
        {
            get { return _current?.ForegroundColor.ToSolidColorBrush(); }
            set
            {
                _current.ForegroundColor = value.ToColor();
                SetFieldValue(EntryFieldName.ForegroundColor, _current.ForegroundColor, false).ConfigureAwait(false).GetAwaiter();
            }
        }
        
        public RelayCommand SaveCommand { get; }
        public RelayCommand<string> MoveCommand { get; }
        public RelayCommand RestoreCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand AddAdditionalField { get; }
        public RelayCommand<EntryFieldVm> DeleteAdditionalField { get; }
        public RelayCommand<Attachment> OpenAttachmentCommand { get; }
        public RelayCommand AddAttachmentCommand { get; }
        public RelayCommand<Attachment> DeleteAttachmentCommand { get; }
        public RelayCommand<EntryVm> SetCurrentEntryCommand { get; }

        private DatabaseVm Database => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
        
        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        private readonly IResourceProxy _resource;
        private readonly IDialogService _dialog;
        private readonly INotificationService _notification;
        private readonly IFileProxy _file;
        private readonly ICryptographyClient _cryptography;
        private readonly IDateTime _dateTime;
        private GroupVm _parent;
        private EntryVm _current;
        private int _selectedIndex;
        private int _additionalFieldSelectedIndex = -1;
        private bool _isDirty;

        public EntryDetailVm(IMediator mediator, INavigationService navigation, IResourceProxy resource, IDialogService dialog, INotificationService notification, IFileProxy file, ICryptographyClient cryptography, IDateTime dateTime)
        {
            _mediator = mediator;
            _navigation = navigation;
            _resource = resource;
            _dialog = dialog;
            _notification = notification;
            _file = file;
            _cryptography = cryptography;
            _dateTime = dateTime;

            SaveCommand = new RelayCommand(async () => await SaveChanges(), () => Database.IsDirty);
            MoveCommand = new RelayCommand<string>(async destination => await Move(destination), destination => _parent != null && !string.IsNullOrEmpty(destination) && destination != _parent.Id);
            RestoreCommand = new RelayCommand(async () => await RestoreHistory());
            DeleteCommand = new RelayCommand(async () => await AskForDelete());
            AddAdditionalField = new RelayCommand(AddField, () => IsCurrentEntry);
            DeleteAdditionalField = new RelayCommand<EntryFieldVm>(async field => await DeleteField(field), field => field != null && IsCurrentEntry);
            OpenAttachmentCommand = new RelayCommand<Attachment>(async attachment => await OpenAttachment(attachment));
            AddAttachmentCommand = new RelayCommand(async () => await AddAttachment(), () => IsCurrentEntry);
            DeleteAttachmentCommand = new RelayCommand<Attachment>(async attachment => await DeleteAttachment(attachment), _ => IsCurrentEntry);
            SetCurrentEntryCommand = new RelayCommand<EntryVm>(SetCurrentEntry, entry => entry != null);

            MessengerInstance.Register<DatabaseSavedMessage>(this, _ => SaveCommand.RaiseCanExecuteChanged());
            MessengerInstance.Register<EntryFieldValueChangedMessage>(this, async message => await SetFieldValue(message.FieldName, message.FieldValue, message.IsProtected));
            MessengerInstance.Register<EntryFieldNameChangedMessage>(this, async message => await UpdateFieldName(message.OldName, message.NewName, message.Value, message.IsProtected));
            MessengerInstance.Register<PasswordGeneratedMessage>(this, message => Password = message.Password);
        }

        public async Task Initialize(string entryId)
        {
            SelectedIndex = 0;
            _current = await _mediator.Send(new GetEntryQuery { Id = entryId });
            SetCurrentEntry(_current);
            _parent = await _mediator.Send(new GetGroupQuery { Id = _current.ParentGroupId });
            History = new ObservableCollection<IEntityVm> { _current };
            foreach (var entry in _current.History.Skip(1))
            {
                History.Add(entry);
            }
            History.CollectionChanged += (sender, args) =>
            {
                SelectedIndex = 0;
                SaveCommand.RaiseCanExecuteChanged();
            };
        }

        public async Task AddHistory()
        {
            if (_isDirty && Database.IsOpen) await _mediator.Send(new AddHistoryCommand { Entry = History[0] as EntryVm });
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
            await _mediator.Send(new UpsertFieldCommand { EntryId = Id, FieldName = fieldName, FieldValue = value, IsProtected = isProtected});
            UpdateDirtyStatus(true);
        }

        private async Task UpdateFieldName(string oldName, string newName, string value, bool isProtected)
        {
            if (!string.IsNullOrEmpty(oldName)) await _mediator.Send(new DeleteFieldCommand { EntryId = Id, FieldName = oldName });
            await SetFieldValue(newName, value, isProtected);
        }
        
        private void AddField()
        {
            AdditionalFields.Add(new EntryFieldVm(_cryptography));
            AdditionalFieldSelectedIndex = AdditionalFields.Count - 1;
        }

        private async Task DeleteField(EntryFieldVm field)
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
                        await _mediator.Send(new DeleteHistoryCommand { Entry = History[0] as EntryVm, HistoryIndex = History.Count - SelectedIndex - 1 });
                        History.RemoveAt(SelectedIndex);
                    });
            }
        }

        private async Task RestoreHistory()
        {
            await _mediator.Send(new RestoreHistoryCommand { Entry = History[0] as EntryVm, HistoryIndex = History.Count - SelectedIndex - 1 });
            History.Insert(0, _current);
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
                ParentGroupId = _current.ParentGroupId,
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
            await _mediator.Send(new AddAttachmentCommand { Entry = _current, AttachmentName = fileInfo.Name, AttachmentContent = contents });
            Attachments.Add(new Attachment { Name = fileInfo.Name, Content = contents });
        }

        private async Task DeleteAttachment(Attachment attachment)
        {
            await _mediator.Send(new DeleteAttachmentCommand { Entry = _current, AttachmentName = attachment.Name });
            Attachments.Remove(attachment);
        }

        private void UpdateDirtyStatus(bool isDirty)
        {
            SaveCommand.RaiseCanExecuteChanged();
            _isDirty = isDirty;
        }

        private void SetCurrentEntry(EntryVm entry)
        {
            _current = entry;
            AdditionalFields =
                new ObservableCollection<EntryFieldVm>(
                    entry.AdditionalFields.Select(f =>
                    {
                        var field = new EntryFieldVm(_cryptography);
                        field.Initialize(f.Name, f.Value, f.IsProtected);
                        return field;
                    }));

            Attachments = new ObservableCollection<Attachment>(entry.Attachments.Select(f => new Attachment
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
