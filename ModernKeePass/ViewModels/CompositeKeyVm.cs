using System.Text;
using Windows.Storage;
using Windows.UI.Xaml;
using ModernKeePass.Common;
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

        private readonly App _app = Application.Current as App;
        private bool _hasPassword;
        private bool _hasKeyFile;
        private string _password = string.Empty;
        private string _status;
        private StatusTypes _statusType;
        private StorageFile _keyFile;
        private string _keyFileText = "Select key file from disk...";

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

        public bool IsValid => HasPassword || HasKeyFile && KeyFile != null;

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
        
        public bool OpenDatabase(bool createNew)
        {
            _app.Database.Open(CreateCompositeKey(), createNew);
            switch (_app.Database.Status)
            {
                case DatabaseHelper.DatabaseStatus.Opened:
                    RootGroup = _app.Database.RootGroup;
                    return true;
                case DatabaseHelper.DatabaseStatus.CompositeKeyError:
                    var errorMessage = new StringBuilder("Error: wrong ");
                    if (HasPassword) errorMessage.Append("password");
                    if (HasPassword && HasKeyFile) errorMessage.Append(" or ");
                    if (HasKeyFile) errorMessage.Append("key file");
                    UpdateStatus(errorMessage.ToString(), StatusTypes.Error);
                    break;
            }
            return false;
        }

        public void UpdateKey()
        {
            _app.Database.UpdateCompositeKey(CreateCompositeKey());
            UpdateStatus("Database composite key updated.", StatusTypes.Success);
        }

        public void CreateKeyFile(StorageFile file)
        {
            // TODO: implement entropy generator
            KcpKeyFile.Create(file, null);
            KeyFile = file;
        }

        private void UpdateStatus(string text, StatusTypes type)
        {
            Status = text;
            StatusType = (int)type;
        }

        private CompositeKey CreateCompositeKey()
        {
            var compositeKey = new CompositeKey();
            if (HasPassword) compositeKey.AddUserKey(new KcpPassword(Password));
            if (HasKeyFile && KeyFile != null) compositeKey.AddUserKey(new KcpKeyFile(IOConnectionInfo.FromFile(KeyFile)));
            return compositeKey;
        }
    }
}
