using System;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Database.Queries.OpenDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.AOP;

namespace ModernKeePass.ViewModels
{
    public class OpenDatabaseControlVm : NotifyPropertyChangedBase
    {
        public enum StatusTypes
        {
            Normal = 0,
            Error = 1,
            Warning = 3,
            Success = 5
        }

        public bool HasPassword
        {
            get { return _hasPassword; }
            set
            {
                SetProperty(ref _hasPassword, value);
                OnPropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
            }
        }

        public bool HasKeyFile
        {
            get { return _hasKeyFile; }
            set
            {
                SetProperty(ref _hasKeyFile, value);
                OnPropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsValid => !_isOpening && (HasPassword || HasKeyFile && KeyFilePath != null);

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public int StatusType
        {
            get { return (int)_statusType; }
            set { SetProperty(ref _statusType, (StatusTypes)value); }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                StatusType = (int)StatusTypes.Normal;
                Status = string.Empty;
            }
        }

        public string KeyFilePath
        {
            get { return _keyFilePath; }
            set
            {
                _keyFilePath = value;
                OnPropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeyFileText
        {
            get { return _keyFileText; }
            set { SetProperty(ref _keyFileText, value); }
        }

        public string OpenButtonLabel
        {
            get { return _openButtonLabel; }
            set { SetProperty(ref _openButtonLabel, value); }
        }
        
        public RelayCommand<string> OpenDatabaseCommand { get; }
        
        protected readonly IMediator Mediator;
        private readonly IResourceProxy _resource;
        private readonly IMessenger _messenger;
        private readonly IDialogService _dialog;
        private bool _hasPassword;
        private bool _hasKeyFile;
        private bool _isOpening;
        private string _password = string.Empty;
        private string _status;
        private StatusTypes _statusType;
        private string _keyFilePath;
        private string _keyFileText;
        private string _openButtonLabel;


        public OpenDatabaseControlVm() : this(
            App.Services.GetRequiredService<IMediator>(),
            App.Services.GetRequiredService<IResourceProxy>(),
            App.Services.GetRequiredService<IMessenger>(),
            App.Services.GetRequiredService<IDialogService>())
        { }

        public OpenDatabaseControlVm(IMediator mediator, IResourceProxy resource, IMessenger messenger, IDialogService dialog)
        {
            Mediator = mediator;
            _resource = resource;
            _messenger = messenger;
            _dialog = dialog;
            OpenDatabaseCommand = new RelayCommand<string>(async databaseFilePath => await TryOpenDatabase(databaseFilePath), _ => IsValid);
            _keyFileText = _resource.GetResourceValue("CompositeKeyDefaultKeyFile");
            _openButtonLabel = _resource.GetResourceValue("CompositeKeyOpenButtonLabel");
        }

        public async Task TryOpenDatabase(string databaseFilePath)
        {
            _messenger.Send(new DatabaseOpeningMessage {Token = databaseFilePath});

            var database = await Mediator.Send(new GetDatabaseQuery());
            if (database.IsOpen)
            {
                await _dialog.ShowMessage(_resource.GetResourceValue("MessageDialogDBOpenTitle"),
                    string.Format(_resource.GetResourceValue("MessageDialogDBOpenDesc"), database.Name),
                    _resource.GetResourceValue("MessageDialogDBOpenButtonSave"),
                    _resource.GetResourceValue("MessageDialogDBOpenButtonDiscard"),
                    async isOk =>
                    {
                        if (isOk)
                        {
                            await Mediator.Send(new SaveDatabaseCommand());
                            ToastNotificationHelper.ShowGenericToast(
                                database.Name,
                                _resource.GetResourceValue("ToastSavedMessage"));
                            await Mediator.Send(new CloseDatabaseCommand());
                            await OpenDatabase(databaseFilePath);
                        }
                        else
                        {
                            await Mediator.Send(new CloseDatabaseCommand());
                            await OpenDatabase(databaseFilePath);
                        }
                    });
            }
            else await OpenDatabase(databaseFilePath);
        }

        public async Task OpenDatabase(string databaseFilePath)
        {
            var oldLabel = _openButtonLabel;
            OpenButtonLabel = _resource.GetResourceValue("CompositeKeyOpening");
            _isOpening = true;
            try
            {
                OnPropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
                await Mediator.Send(new OpenDatabaseQuery
                {
                    FilePath = databaseFilePath,
                    KeyFilePath = HasKeyFile ? KeyFilePath : null,
                    Password = HasPassword ? Password : null,
                });
                var rootGroupId = (await Mediator.Send(new GetDatabaseQuery())).RootGroupId;

                _messenger.Send(new DatabaseOpenedMessage { RootGroupId = rootGroupId });
            }
            catch (ArgumentException)
            {
                var errorMessage = new StringBuilder($"{_resource.GetResourceValue("CompositeKeyErrorOpen")}\n");
                if (HasPassword) errorMessage.AppendLine(_resource.GetResourceValue("CompositeKeyErrorUserPassword"));
                if (HasKeyFile) errorMessage.AppendLine(_resource.GetResourceValue("CompositeKeyErrorUserKeyFile"));
                UpdateStatus(errorMessage.ToString(), StatusTypes.Error);
            }
            catch (Exception e)
            {
                var error = $"{_resource.GetResourceValue("CompositeKeyErrorOpen")}{e.Message}";
                UpdateStatus(error, StatusTypes.Error);
            }
            finally
            {
                _isOpening = false;
                OnPropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
                OpenButtonLabel = oldLabel;
            }
        }

        private void UpdateStatus(string text, StatusTypes type)
        {
            Status = text;
            StatusType = (int)type;
        }
    }
}
