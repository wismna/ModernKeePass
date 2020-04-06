using System;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class CredentialsViewModel: NotifyPropertyChangedBase
    {

        private bool _hasPassword;
        private bool _hasKeyFile;
        private bool _hasUserAccount;
        private bool _isOpening;
        private string _password = string.Empty;
        private string _status;
        private CredentialStatusTypes _statusType;
        private string _keyFilePath = string.Empty;
        private string _keyFileText;
        private readonly IResourceService _resourceService;
        private readonly IDatabaseService _databaseService;

        public bool HasPassword
        {
            get => _hasPassword;
            set
            {
                SetProperty(ref _hasPassword, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public bool HasKeyFile
        {
            get => _hasKeyFile;
            set
            {
                SetProperty(ref _hasKeyFile, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public bool HasUserAccount
        {
            get => _hasUserAccount;
            set
            {
                SetProperty(ref _hasUserAccount, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid => !_isOpening && (HasPassword || HasKeyFile && KeyFilePath != string.Empty || HasUserAccount);

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public int StatusType
        {
            get => (int)_statusType;
            set => SetProperty(ref _statusType, (CredentialStatusTypes)value);
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                StatusType = (int)CredentialStatusTypes.Normal;
                Status = string.Empty;
            }
        }

        public string KeyFilePath
        {
            get => _keyFilePath;
            set
            {
                _keyFilePath = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public string KeyFileText
        {
            get => _keyFileText;
            set => SetProperty(ref _keyFileText, value);
        }
        

        public CredentialsViewModel() : this(App.Container.Resolve<IDatabaseService>(), App.Container.Resolve<IResourceService>()) { }

        public CredentialsViewModel(IDatabaseService databaseService, IResourceService resourceService)
        {
            _databaseService = databaseService;
            _resourceService = resourceService;
            _keyFileText = _resourceService.GetResourceValue("CompositeKeyDefaultKeyFile");
        }

        public async Task<bool> OpenDatabase(FileInfo fileInfo)
        {
            try
            {
                _isOpening = true;
                OnPropertyChanged(nameof(IsValid));
                var credentials = new Credentials
                {
                    KeyFilePath = HasKeyFile ? KeyFilePath : string.Empty,
                    Password = HasPassword ? Password : string.Empty
                };
                await Task.Run(() => _databaseService.Open(fileInfo, credentials));
                return true;
            }
            catch (ArgumentException)
            {
                var errorMessage = new StringBuilder($"{_resourceService.GetResourceValue("CompositeKeyErrorOpen")}\n");
                if (HasPassword) errorMessage.AppendLine(_resourceService.GetResourceValue("CompositeKeyErrorUserPassword"));
                if (HasKeyFile) errorMessage.AppendLine(_resourceService.GetResourceValue("CompositeKeyErrorUserKeyFile"));
                if (HasUserAccount) errorMessage.AppendLine(_resourceService.GetResourceValue("CompositeKeyErrorUserAccount"));
                UpdateStatus(errorMessage.ToString(), CredentialStatusTypes.Error);
            }
            catch (Exception e)
            {
                var error = $"{_resourceService.GetResourceValue("CompositeKeyErrorOpen")}{e.Message}";
                UpdateStatus(error, CredentialStatusTypes.Error);
            }
            finally
            {
                _isOpening = false;
                OnPropertyChanged(nameof(IsValid));
            }
            return false;
        }

        private void UpdateStatus(string text, CredentialStatusTypes type)
        {
            Status = text;
            StatusType = (int)type;
        }
    }
}
