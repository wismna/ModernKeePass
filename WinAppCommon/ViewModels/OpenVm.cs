using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Common;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: ViewModelBase
    {
        private readonly IFileProxy _file;
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

        public RelayCommand OpenDatabaseFileCommand { get; }

        public OpenVm(IFileProxy file)
        {
            _file = file;
            OpenDatabaseFileCommand = new RelayCommand(async () => await OpenDatabaseFile());
        }

        public void SetFileInformation(FileInfo file)
        {
            if (file == null) return;
            Token = file.Id;
            Path = file.Path;
            Name = file.Name;
        }

        private async Task OpenDatabaseFile()
        {
            var file = await _file.OpenFile(string.Empty, Constants.Extensions.Kdbx, true);
            SetFileInformation(file);
        }
    }
}
