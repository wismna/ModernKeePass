using System;
using System.IO;
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

        public string Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                IsError = false;
                Status = string.Empty;
            }
        }

        public string KeyFilePath
        {
            get { return _keyFilePath; }
            set
            {
                _keyFilePath = value;
                IsError = false;
                RaisePropertyChanged(nameof(IsValid));
                OpenDatabaseCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeyFileText
        {
            get { return _keyFileText; }
            set { Set(() => KeyFileText, ref _keyFileText, value); }
        }

        public bool IsOpening
        {
            get { return _isOpening; }
            set { Set(() => IsOpening, ref _isOpening, value); }
        }

        public bool IsError
        {
            get { return _isError; }
            set { Set(() => IsError, ref _isError, value); }
        }

        public bool IsValid => !IsOpening && (HasPassword || HasKeyFile && !string.IsNullOrEmpty(KeyFilePath));

        public RelayCommand<string> OpenDatabaseCommand { get; }

        private readonly IMediator _mediator;
        private readonly IResourceProxy _resource;
        private readonly INotificationService _notification;
        private bool _hasPassword;
        private bool _hasKeyFile;
        private bool _isOpening;
        private string _password = string.Empty;
        private string _status;
        private string _keyFilePath;
        private string _keyFileText;
        private bool _isError;

        public OpenDatabaseControlVm(IMediator mediator, IResourceProxy resource, INotificationService notification)
        {
            _mediator = mediator;
            _resource = resource;
            _notification = notification;
            OpenDatabaseCommand = new RelayCommand<string>(async databaseFilePath => await TryOpenDatabase(databaseFilePath), _ => IsValid);
            _keyFileText = _resource.GetResourceValue("CompositeKeyDefaultKeyFile");
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
            IsOpening = true;
            try
            {
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
                Status = errorMessage.ToString();
                IsError = true;
            }
            catch (FileNotFoundException)
            {
                _notification.Show(
                    $"{_resource.GetResourceValue("FileNotFoundTitle")}", 
                    $"{_resource.GetResourceValue("FileNotFoundDescription")}");
                MessengerInstance.Send(new FileNotFoundMessage());
                IsError = true;
            }
            catch (Exception e)
            {
                Status = $"{_resource.GetResourceValue("CompositeKeyErrorOpen")}{e.Message}";
                IsError = true;
            }
            finally
            {
                IsOpening = false;
            }
        }
    }
}
