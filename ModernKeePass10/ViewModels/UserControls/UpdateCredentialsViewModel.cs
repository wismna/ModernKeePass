using Autofac;
using System;
using System.Threading.Tasks;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class UpdateCredentialsViewModel : NotifyPropertyChangedBase
    {
        private readonly ISecurityService _securityService;
        private bool _hasPassword;
        private bool _hasKeyFile;
        private bool _hasUserAccount;
        private string _confirmPassword;
        private string _password;
        private string _keyFileText;
        private string _status;
        private CredentialStatusTypes _statusType;

        public string Password
        {
            get => _password;
            set
            { 
                SetProperty(ref _password, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string KeyFilePath { get; set; }

        public bool HasPassword
        {
            get => _hasPassword;
            set => SetProperty(ref _hasPassword, value);
        }

        public bool HasKeyFile
        {
            get => _hasKeyFile;
            set => SetProperty(ref _hasKeyFile, value);
        }

        public bool HasUserAccount
        {
            get => _hasUserAccount;
            set => SetProperty(ref _hasUserAccount, value);
        }

        public string KeyFileText
        {
            get => _keyFileText;
            set => SetProperty(ref _keyFileText, value);
        }

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

        public double PasswordComplexityIndicator => _securityService.EstimatePasswordComplexity(Password);

        public bool IsValid => HasPassword && Password == ConfirmPassword || HasKeyFile && KeyFilePath != string.Empty || HasUserAccount;

        public UpdateCredentialsViewModel(): this(App.Container.Resolve<ISecurityService>()) { }

        public UpdateCredentialsViewModel(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        internal Task<bool> CreateDatabase(FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}