using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Security.Commands.GenerateKeyFile;
using ModernKeePass.Domain.Common;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.ViewModels
{
    public class SetCredentialsVm : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly IResourceProxy _resource;
        private readonly IFileProxy _file;

        public bool HasPassword
        {
            get { return _hasPassword; }
            set
            {
                Set(() => HasPassword, ref _hasPassword, value);
                RaisePropertyChanged(nameof(IsPasswordValid));
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
                RaisePropertyChanged(nameof(IsKeyFileValid));
                RaisePropertyChanged(nameof(IsValid));
                OpenKeyFileCommand.RaiseCanExecuteChanged();
                CreateKeyFileCommand.RaiseCanExecuteChanged();
                GenerateCredentialsCommand.RaiseCanExecuteChanged();
            }
        }
        
        public string Password
        {
            get { return _password; }
            set
            {
                Set(() => Password, ref _password, value);
                RaisePropertyChanged(nameof(IsPasswordValid));
                RaisePropertyChanged(nameof(IsValid));
                GenerateCredentialsCommand.RaiseCanExecuteChanged();
            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                Set(() => ConfirmPassword, ref _confirmPassword, value);
                RaisePropertyChanged(nameof(IsPasswordValid));
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
                RaisePropertyChanged(nameof(IsKeyFileValid));
                RaisePropertyChanged(nameof(IsValid));
                GenerateCredentialsCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeyFileText
        {
            get { return _keyFileText; }
            set { Set(() => KeyFileText, ref _keyFileText, value); }
        }
        
        public bool IsPasswordValid => HasPassword && Password == ConfirmPassword || !HasPassword;
        public bool IsKeyFileValid => HasKeyFile && !string.IsNullOrEmpty(KeyFilePath) || !HasKeyFile;
        public bool IsValid => HasPassword && Password == ConfirmPassword || HasKeyFile && !string.IsNullOrEmpty(KeyFilePath) && (HasPassword || HasKeyFile);

        public RelayCommand OpenKeyFileCommand { get; }
        public RelayCommand CreateKeyFileCommand { get; }
        public RelayCommand GenerateCredentialsCommand{ get; }

        private bool _hasPassword;
        private bool _hasKeyFile;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _keyFilePath;
        private string _keyFileText;
        
        public SetCredentialsVm(IMediator mediator, IResourceProxy resource, IFileProxy file)
        {
            _mediator = mediator;
            _resource = resource;
            _file = file;

            OpenKeyFileCommand = new RelayCommand(async () => await OpenKeyFile(), () => HasKeyFile);
            CreateKeyFileCommand = new RelayCommand(async () => await CreateKeyFile(), () => HasKeyFile);
            GenerateCredentialsCommand = new RelayCommand(GenerateCredentials, () => IsValid);

            _keyFileText = resource.GetResourceValue("CompositeKeyDefaultKeyFile");

            MessengerInstance.Register<PasswordGeneratedMessage>(this, message =>
            {
                Password = message.Password;
                ConfirmPassword = message.Password;
            });
        }
        
        private async Task OpenKeyFile()
        {
            var file = await _file.OpenFile(string.Empty, Constants.Extensions.Any, false);
            if (file == null) return;
            SetKeyFileInfo(file);
        }

        private async Task CreateKeyFile()
        {
            var file = await _file.CreateFile(_resource.GetResourceValue("CompositeKeyFileNameSuggestion"),
                Constants.Extensions.Key, _resource.GetResourceValue("CompositeKeyFileTypeDesc"), 
                false);
            if (file == null) return;
            SetKeyFileInfo(file);

            await _mediator.Send(new GenerateKeyFileCommand { KeyFilePath = KeyFilePath });
        }

        private void SetKeyFileInfo(FileInfo file)
        {
            KeyFilePath = file.Id;
            KeyFileText = file.Name;
            HasKeyFile = true;
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