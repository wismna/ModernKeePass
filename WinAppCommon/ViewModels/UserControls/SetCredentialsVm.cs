using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Security.Commands.GenerateKeyFile;

namespace ModernKeePass.ViewModels
{
    public class SetCredentialsVm : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly ICredentialsProxy _credentials;
        
        public bool HasPassword
        {
            get { return _hasPassword; }
            set
            {
                Set(() => HasPassword, ref _hasPassword, value);
                RaisePropertyChanged(nameof(IsValid));
                GenerateCredentialsCommand.RaiseCanExecuteChanged();
            }
        }

        public bool HasKeyFile
        {
            get { return _hasKeyFile; }
            set
            {
                Set(() => HasKeyFile, ref _hasKeyFile, value);
                RaisePropertyChanged(nameof(IsValid));
                GenerateCredentialsCommand.RaiseCanExecuteChanged();
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
                RaisePropertyChanged(nameof(IsValid));
                RaisePropertyChanged(nameof(PasswordComplexityIndicator));
                GenerateCredentialsCommand.RaiseCanExecuteChanged();
            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged(nameof(IsValid));
                GenerateCredentialsCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeyFilePath
        {
            get { return _keyFilePath; }
            set
            {
                _keyFilePath = value;
                RaisePropertyChanged(nameof(IsValid));
                GenerateCredentialsCommand.RaiseCanExecuteChanged();
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

        public double PasswordComplexityIndicator => _credentials.EstimatePasswordComplexity(Password);

        public bool IsValid => HasPassword && Password == ConfirmPassword || HasKeyFile && !string.IsNullOrEmpty(KeyFilePath);

        public RelayCommand GenerateCredentialsCommand{ get; }

        private bool _hasPassword;
        private bool _hasKeyFile;
        private string _password = string.Empty;
        private string _confirmPassword;
        private string _status;
        private string _keyFilePath;
        private string _keyFileText;
        private string _openButtonLabel;
        
        public SetCredentialsVm(IMediator mediator, ICredentialsProxy credentials, IResourceProxy resource)
        {
            _mediator = mediator;
            _credentials = credentials;
            GenerateCredentialsCommand = new RelayCommand(GenerateCredentials, () => IsValid);

            _keyFileText = resource.GetResourceValue("CompositeKeyDefaultKeyFile");
        }

        public async Task GenerateKeyFile()
        {
            await _mediator.Send(new GenerateKeyFileCommand {KeyFilePath = KeyFilePath});
        }

        private void GenerateCredentials()
        {
            MessengerInstance.Send(new CredentialsSetMessage
            {
                Password = HasPassword ? Password : null,
                KeyFilePath = HasKeyFile ? KeyFilePath : null
            });
        }
    }
}