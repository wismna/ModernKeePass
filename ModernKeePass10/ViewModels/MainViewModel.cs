using Windows.Storage;
using Autofac;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class MainViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IRecentService _recentService;

        public bool IsDatabaseOpened => _databaseService.IsOpen;
        public bool HasRecentItems => _recentService.HasEntries;

        public string OpenedDatabaseName => _databaseService.Name;
        public IStorageFile File { get; set; }

        public MainViewModel()
        {
            _databaseService = App.Container.Resolve<IDatabaseService>();
            _recentService = App.Container.Resolve<IRecentService>();
        }

        public MainViewModel(IDatabaseService databaseService, IRecentService recentService)
        {
            _databaseService = databaseService;
            _recentService = recentService;
        }
    }
}