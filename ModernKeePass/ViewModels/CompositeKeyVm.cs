using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using MediatR;
using ModernKeePass.Application.Database.Commands.UpdateCredentials;
using ModernKeePass.Application.Database.Queries.OpenDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Keys;

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

        public bool IsValid => !_isOpening && (HasPassword || HasKeyFile && KeyFile != null || HasUserAccount);

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
                OnPropertyChanged("PasswordComplexityIndicator");
                StatusType = (int)StatusTypes.Normal;
                Status = string.Empty;
            }
        }

        public StorageFile KeyFile
        {
            get { return _keyFile; }
            set
            {
                _keyFile = value;
                KeyFileText = value?.Name;
                OnPropertyChanged("IsValid");
            }
        }

        public string KeyFileText
        {
            get { return _keyFileText; }
            set { SetProperty(ref _keyFileText, value); }
        }

        public GroupVm RootGroup { get; set; }

        public double PasswordComplexityIndicator => QualityEstimation.EstimatePasswordBits(Password?.ToCharArray());
        
        private bool _hasPassword;
        private bool _hasKeyFile;
        private bool _hasUserAccount;
        private bool _isOpening;
        private string _password = string.Empty;
        private string _status;
        private StatusTypes _statusType;
        private StorageFile _keyFile;
        private string _keyFileText;
        private readonly IMediator _mediator;
        private readonly IResourceService _resource;

        public CompositeKeyVm() : this(App.Mediator, new ResourcesService()) { }

        public CompositeKeyVm(IMediator mediator, IResourceService resource)
        {
            _mediator = mediator;
            _resource = resource;
            _keyFileText = _resource.GetResourceValue("CompositeKeyDefaultKeyFile");
        }

        public async Task<bool> OpenDatabase(StorageFile databaseFile, bool createNew)
        {
            try
            {
                _isOpening = true;
                OnPropertyChanged("IsValid");;
                var fileInfo = new FileInfo
                {
                    Name = databaseFile.DisplayName,
                    Path = StorageApplicationPermissions.FutureAccessList.Add(databaseFile)
                };

                var database = await _mediator.Send(new OpenDatabaseQuery { FileInfo = fileInfo, Credentials = CreateCredentials()});
                await Task.Run(() => RootGroup = new GroupVm(database.RootGroup));
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
            //Database.UpdateCompositeKey(await CreateCompositeKey());
            await _mediator.Send(new UpdateCredentialsCommand {Credentials = CreateCredentials()});
            UpdateStatus(_resource.GetResourceValue("CompositeKeyUpdated"), StatusTypes.Success);
        }

        public async Task CreateKeyFile(StorageFile file)
        {
            // TODO: implement entropy generator
            var fileContents = await FileIO.ReadBufferAsync(file);
            KcpKeyFile.Create(fileContents.ToArray());
            KeyFile = file;
        }

        private void UpdateStatus(string text, StatusTypes type)
        {
            Status = text;
            StatusType = (int)type;
        }

        private Credentials CreateCredentials()
        {
            var credentials = new Credentials
            {
                Password = HasPassword ? Password: null,
                KeyFilePath = HasKeyFile && KeyFile != null ? StorageApplicationPermissions.FutureAccessList.Add(KeyFile) : null
            };
            return credentials;
        }
    }
}
