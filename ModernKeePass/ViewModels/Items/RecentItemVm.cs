using System.Threading.Tasks;
using Windows.Storage;
using ModernKeePass.Common;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class RecentItemVm: NotifyPropertyChangedBase, ISelectableModel, IRecentItem
    {
        private bool _isSelected;
        
        public StorageFile DatabaseFile { get; }
        public string Token { get; }
        public string Name { get; }
        public string Path => DatabaseFile?.Path;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public RecentItemVm() {}
        public RecentItemVm(string token, string metadata, IStorageItem file)
        {
            Token = token;
            Name = metadata;
            DatabaseFile = file as StorageFile;
        }
        
        public void UpdateAccessTime()
        {
            UpdateAccessTime(RecentService.Instance).Wait();
        }

        public async Task UpdateAccessTime(IRecentService recent)
        {
            await recent.GetFileAsync(Token);
        }
    }
}
