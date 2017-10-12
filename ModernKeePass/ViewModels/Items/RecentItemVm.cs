using System;
using Windows.Storage;
using ModernKeePass.Common;
using Windows.Storage.AccessCache;

namespace ModernKeePass.ViewModels
{
    public class RecentItemVm: NotifyPropertyChangedBase
    {
        private bool _isSelected;
        
        public RecentItemVm(AccessListEntry entry, StorageFile file)
        {
            Token = entry.Token;
            Name = entry.Metadata;
            DatabaseFile = file;
        }

        public StorageFile DatabaseFile { get; private set; }
        public string Token { get; private set; }
        public string Name { get; private set; }
        public string Path => DatabaseFile.Path;

        public bool IsSelected
        {
            get { return _isSelected; }
            internal set { SetProperty(ref _isSelected, value); }
        }
    }
}
