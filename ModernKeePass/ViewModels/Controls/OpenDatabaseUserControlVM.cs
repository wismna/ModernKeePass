using ModernKeePass.Common;
using ModernKeePassLib.Cryptography;

namespace ModernKeePass.ViewModels
{
    public class OpenDatabaseUserControlVm: NotifyPropertyChangedBase
    {
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("PasswordComplexityIndicator");
            }
        }

        public double PasswordComplexityIndicator => QualityEstimation.EstimatePasswordBits(Password.ToCharArray());
    }
}
