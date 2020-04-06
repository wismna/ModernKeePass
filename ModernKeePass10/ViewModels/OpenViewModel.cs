using System.Threading.Tasks;
using Autofac;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class OpenViewModel: NotifyPropertyChangedBase
    {
        private readonly IRecentService _recentService;
        private string _name;
        private string _databaseFilePath;

        public bool IsFileSelected => !string.IsNullOrEmpty(DatabaseFilePath);

        public string Name
        {
            get => _name;
            private set => SetProperty(ref _name, value);
        }

        public string DatabaseFilePath
        {
            get => _databaseFilePath;
            private set => SetProperty(ref _databaseFilePath, value);
        }

        public OpenViewModel(): this(App.Container.Resolve<IRecentService>())
        { }
        public OpenViewModel(IRecentService recentService)
        {
            _recentService = recentService;
        }
        
        public async Task OpenFile(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            DatabaseFilePath = fileInfo.Path;
            OnPropertyChanged(nameof(IsFileSelected));
            await _recentService.Add(fileInfo);
        }
    }
}
