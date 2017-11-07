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
        private string _password;
        private string _status;
        private StatusTypes _statusType;
        private StorageFile _keyFile;

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

        public bool IsValid => HasPassword || HasKeyFile;

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
                UpdateStatus($"Key file: {value.Name}", StatusTypes.Normal);
            }
        }

        public GroupVm RootGroup { get; set; }
        public double PasswordComplexityIndicator => QualityEstimation.EstimatePasswordBits(Password?.ToCharArray());
        
        public DatabaseHelper.DatabaseStatus OpenDatabase(bool createNew)
        {
            UpdateStatus(_app.Database.Open(CreateCompositeKey(), createNew), StatusTypes.Error);
            RootGroup = _app.Database.RootGroup;
            return _app.Database.Status;
        }

        public void UpdateKey()
        {
            _app.Database.UpdateCompositeKey(CreateCompositeKey());
            UpdateStatus("Database composite key updated.", StatusTypes.Success);
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
            if (HasKeyFile) compositeKey.AddUserKey(new KcpKeyFile(IOConnectionInfo.FromFile(KeyFile)));
            return compositeKey;
        }
    }
}
