using GalaSoft.MvvmLight;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: ViewModelBase
    {
        private string _name;
        private string _path;
        private string _token;
        public bool IsFileSelected => !string.IsNullOrEmpty(Path);

        public string Token
        {
            get { return _token; }
            set { Set(() => Token, ref _token, value); }
        }

        public string Name
        {
            get { return _name; }
            private set { Set(() => Name, ref _name, value); }
        }

        public string Path  
        {
            get { return _path; }
            private set { Set(() => Path, ref _path, value); }
        }
        
        public void OpenFile(FileInfo file)
        {
            Token = file.Id;
            Name = file.Name;
            Path = file.Path;
            RaisePropertyChanged(nameof(IsFileSelected));
        }
    }
}
