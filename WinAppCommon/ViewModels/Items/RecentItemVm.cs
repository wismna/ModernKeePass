using GalaSoft.MvvmLight;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class RecentItemVm: ObservableObject, ISelectableModel
    {
        private bool _isSelected;
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

        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(() => IsSelected, ref _isSelected, value); }
        }
        
        public RecentItemVm(FileInfo file)
        {
            Token = file.Id;
            Name = file.Name;
            Path = file.Path;
        }
    }
}
