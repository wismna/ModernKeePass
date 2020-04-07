using System;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Database.Commands.UpdateCredentials;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Database.Queries.OpenDatabase;
using ModernKeePass.Application.Security.Commands.GenerateKeyFile;
using ModernKeePass.Application.Security.Queries.EstimatePasswordComplexity;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels
{
    public class CompositeKeyVm: NotifyPropertyChangedBase
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
                OnPropertyChanged("IsValid");
            }
        }

        public bool HasKeyFile
        {
            get { return _hasKeyFile; }
            set
            {
                SetProperty(ref _hasKeyFile, value);
                OnPropertyChanged("IsValid");
            }
        }

        public bool HasUserAccount
        {
            get { return _hasUserAccount; }
            set
            {
                SetProperty(ref _hasUserAccount, value);
                OnPropertyChanged("IsValid");
            }
        }

        public bool IsValid => !_isOpening && (HasPassword || HasKeyFile && KeyFilePath != null || HasUserAccount);

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
                OnPropertyChanged(nameof(PasswordComplexityIndicator));
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
            }
        }

        public string KeyFileText
        {
            get { return _keyFileText; }
            set { SetProperty(ref _keyFileText, value); }
        }

        public string RootGroupId { get; set; }

        public double PasswordComplexityIndicator => _mediator.Send(new EstimatePasswordComplexityQuery { Password = Password }).GetAwaiter().GetResult();

        private bool _hasPassword;
        private bool _hasKeyFile;
        private bool _hasUserAccount;
        private bool _isOpening;
        private string _password = string.Empty;
        private string _status;
        private StatusTypes _statusType;
        private string _keyFilePath;
        private string _keyFileText;
        private readonly IMediator _mediator;
        private readonly ResourceHelper _resource;

        public CompositeKeyVm() : this(App.Services.GetService<IMediator>()) { }

        public CompositeKeyVm(IMediator mediator)
        {
            _mediator = mediator;
            _resource = new ResourceHelper();
            _keyFileText = _resource.GetResourceValue("CompositeKeyDefaultKeyFile");
        }

        public async Task<bool> OpenDatabase(string databaseFilePath, bool createNew)
        {
            try
            {
                _isOpening = true;
                OnPropertyChanged(nameof(IsValid));

                await _mediator.Send(new OpenDatabaseQuery {
                    FilePath = databaseFilePath,
                    KeyFilePath = HasKeyFile ? KeyFilePath : null,
                    Password = HasPassword ? Password : null,
                });
                RootGroupId = (await _mediator.Send(new GetDatabaseQuery())).RootGroupId;
                return true;
            }
            catch (ArgumentException)
            {
                var errorMessage = new StringBuilder($"{_resource.GetResourceValue("CompositeKeyErrorOpen")}\n");
                if (HasPassword) errorMessage.AppendLine(_resource.GetResourceValue("CompositeKeyErrorUserPassword"));
                if (HasKeyFile) errorMessage.AppendLine(_resource.GetResourceValue("CompositeKeyErrorUserKeyFile"));
                if (HasUserAccount) errorMessage.AppendLine(_resource.GetResourceValue("CompositeKeyErrorUserAccount"));
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
                OnPropertyChanged("IsValid");
            }
            return false;
        }

        public async Task UpdateKey()
        {
            await _mediator.Send(new UpdateCredentialsCommand
            {
                KeyFilePath = HasKeyFile ? KeyFilePath : null,
                Password = HasPassword ? Password : null,
            });
            UpdateStatus(_resource.GetResourceValue("CompositeKeyUpdated"), StatusTypes.Success);
        }

        public async Task CreateKeyFile(FileInfo file)
        {
            // TODO: implement entropy generator
            await _mediator.Send(new GenerateKeyFileCommand {KeyFilePath = file.Id});
            KeyFilePath = file.Path;
            KeyFileText = file.Name;
        }

        private void UpdateStatus(string text, StatusTypes type)
        {
            Status = text;
            StatusType = (int)type;
        }
    }
}
