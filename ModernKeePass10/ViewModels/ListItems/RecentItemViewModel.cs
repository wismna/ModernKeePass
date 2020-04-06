using System.Threading.Tasks;
using Autofac;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class RecentItemViewModel: NotifyPropertyChangedBase, ISelectableModel
    {
        private readonly IRecentService _recentService;
        private bool _isSelected;
        
        public string Token { get; set; }
        public string Name { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public RecentItemViewModel(): this (App.Container.Resolve<IRecentService>())
        { }

        public RecentItemViewModel(IRecentService recentService)
        {
            _recentService = recentService;
        }
        
        public async Task UpdateAccessTime()
        {
            await _recentService.Get(Token);
        }
    }
}
