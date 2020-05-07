using GalaSoft.MvvmLight;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.ViewModels.ListItems
{
    public class RecentItemVm: ObservableObject
    {
        private string _name;
        private string _token;
        private string _path;

        public string Token
        {
            get { return _token; }
            set { Set(() => Token, ref _token, value); }
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

        public RecentItemVm(FileInfo file)
        {
            Token = file.Id;
            Name = file.Name;
            Path = file.Path;
        }
    }
}
