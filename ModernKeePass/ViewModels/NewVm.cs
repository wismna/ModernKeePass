using ModernKeePassLib.Cryptography;

namespace ModernKeePass.ViewModels
{
    public class NewVm : OpenVm
    {
        private string _password = string.Empty;

        public double PasswordComplexityIndicator => QualityEstimation.EstimatePasswordBits(Password.ToCharArray());
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyPropertyChanged("PasswordComplexityIndicator");
            }
        }

    }
}
