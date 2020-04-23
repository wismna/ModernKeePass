using System;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Database.Queries.OpenDatabase;

namespace ModernKeePass.ViewModels
{
    public class OpenDatabaseControlVm : ViewModelBase
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
                Set(() => HasPassword, ref _hasPassword, value);
                RaisePropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
            }
        }

        public bool HasKeyFile
        {
            get { return _hasKeyFile; }
            set
            {
                Set(() => HasKeyFile, ref _hasKeyFile, value);
                RaisePropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsValid => !_isOpening && (HasPassword || HasKeyFile && !string.IsNullOrEmpty(KeyFilePath));

        public string Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }

        public int StatusType
        {
            get { return _statusType; }
            set { Set(() => StatusType, ref _statusType, value); }
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
                RaisePropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeyFileText
        {
            get { return _keyFileText; }
            set { Set(() => KeyFileText, ref _keyFileText, value); }
        }

        public string OpenButtonLabel
        {
            get { return _openButtonLabel; }
            set { Set(() => OpenButtonLabel, ref _openButtonLabel, value); }
        }
        
        public RelayCommand<string> OpenDatabaseCommand { get; }
        
        private readonly IMediator _mediator;
        private readonly IResourceProxy _resource;
        private bool _hasPassword;
        private bool _hasKeyFile;
        private bool _isOpening;
        private string _password = string.Empty;
        private string _status;
        private int _statusType;
        private string _keyFilePath;
        private string _keyFileText;
        private string _openButtonLabel;
        
        public OpenDatabaseControlVm(IMediator mediator, IResourceProxy resource)
        {
            _mediator = mediator;
            _resource = resource;
            OpenDatabaseCommand = new RelayCommand<string>(async databaseFilePath => await TryOpenDatabase(databaseFilePath), _ => IsValid);
            _keyFileText = _resource.GetResourceValue("CompositeKeyDefaultKeyFile");
            _openButtonLabel = _resource.GetResourceValue("CompositeKeyOpenButtonLabel");
        }

        public async Task TryOpenDatabase(string databaseFilePath)
        {
            MessengerInstance.Send(new DatabaseOpeningMessage {Token = databaseFilePath});

            var database = await _mediator.Send(new GetDatabaseQuery());
            if (database.IsDirty)
            {
                MessengerInstance.Register<DatabaseClosedMessage>(this, async message => await OpenDatabase(message.Parameter as string));
                MessengerInstance.Send(new DatabaseAlreadyOpenedMessage {Parameter = databaseFilePath});
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
                RaisePropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
                await _mediator.Send(new OpenDatabaseQuery
                {
                    FilePath = databaseFilePath,
                    KeyFilePath = HasKeyFile ? KeyFilePath : null,
                    Password = HasPassword ? Password : null,
                });
                var rootGroupId = (await _mediator.Send(new GetDatabaseQuery())).RootGroupId;

                MessengerInstance.Send(new DatabaseOpenedMessage { RootGroupId = rootGroupId });
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
                RaisePropertyChanged(nameof(IsValid));
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
