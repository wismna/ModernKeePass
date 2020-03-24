using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Serialization;

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

        public IDatabaseService Database { get; set; }

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
        private readonly IResourceService _resource;

        public CompositeKeyVm() : this(DatabaseService.Instance, new ResourcesService()) { }

        public CompositeKeyVm(IDatabaseService database, IResourceService resource)
        {
            _resource = resource;
            _keyFileText = _resource.GetResourceValue("CompositeKeyDefaultKeyFile");
            Database = database;
        }

        public async Task<bool> OpenDatabase(StorageFile databaseFile, bool createNew)
        {
            try
            {
                _isOpening = true;
                OnPropertyChanged("IsValid");
                await Database.Open(databaseFile, await CreateCompositeKey(), createNew);
                await Task.Run(() => RootGroup = Database.RootGroup);
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
           Database.UpdateCompositeKey(await CreateCompositeKey());
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

        private async Task<CompositeKey> CreateCompositeKey()
        {
            var compositeKey = new CompositeKey();
            if (HasPassword) compositeKey.AddUserKey(new KcpPassword(Password));
            if (HasKeyFile && KeyFile != null)
            {
                var fileContents = await FileIO.ReadBufferAsync(KeyFile);
                compositeKey.AddUserKey(new KcpKeyFile(IOConnectionInfo.FromByteArray(fileContents.ToArray())));
            }
            if (HasUserAccount) compositeKey.AddUserKey(new KcpUserAccount());
            return compositeKey;
        }
    }
}
