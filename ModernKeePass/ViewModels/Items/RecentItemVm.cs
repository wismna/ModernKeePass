using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class RecentItemVm: NotifyPropertyChangedBase, ISelectableModel
    {
        private readonly IRecentProxy _recent;
        private bool _isSelected;
        
        public string Token { get; }
        public string Name { get; }
        public string Path => string.Empty;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public RecentItemVm(FileInfo file): this(App.Services.GetService<IRecentProxy>(), file) {}
        public RecentItemVm(IRecentProxy recent, FileInfo file)
        {
            _recent = recent;
            Token = file.Path;
            Name = file.Name;
        }
        
        public async Task UpdateAccessTime()
        {
            await _recent.Get(Token);
        }
    }
}
