using GalaSoft.MvvmLight;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: ViewModelBase
    {
        private string _name;
        private string _path;
        private string _token;
        public bool IsFileSelected => !string.IsNullOrEmpty(Token);

        public string Token
        {
            get { return _token; }
            set
            {
                Set(() => Token, ref _token, value);
                RaisePropertyChanged(nameof(IsFileSelected));
            }
        }

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public string Path  
        {
            get { return _path; }
            set { Set(() => Path, ref _path, value); }
        }
    }
}
