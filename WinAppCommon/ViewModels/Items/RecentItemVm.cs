using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class RecentItemVm: NotifyPropertyChangedBase, ISelectableModel
    {
        private bool _isSelected;
        private string _name;
        private string _token;
        private string _path;

        public string Token
        {
            get { return _token; }
            set { SetProperty(ref _token, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
        
        public RecentItemVm(FileInfo file)
        {
            Token = file.Id;
            Name = file.Name;
            Path = file.Path;
        }
    }
}
