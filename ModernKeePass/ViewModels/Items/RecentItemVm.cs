using Windows.Storage;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels
{
    public class RecentItemVm: NotifyPropertyChangedBase
    {
        private bool _isSelected;
        public string Token { get; set; }
        public string Name { get; set; }
        public StorageFile File { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            internal set { SetProperty(ref _isSelected, value); }
        }
    }
}
