using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using GalaSoft.MvvmLight.Views;
using MediatR;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace ModernKeePass.ViewModels
{
    public class SaveVm
    {
        public bool IsSaveEnabled => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult().IsDirty;

        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }

        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        
        public SaveVm(IMediator mediator, INavigationService navigation)
        {
            _mediator = mediator;
            _navigation = navigation;
            SaveCommand = new RelayCommand(async () => await Save(), () => IsSaveEnabled);
            CloseCommand = new RelayCommand(async () => await Close());
        }

        public async Task Save(bool close = true)
        {
            await _mediator.Send(new SaveDatabaseCommand());
            if (close) await _mediator.Send(new CloseDatabaseCommand());
            _navigation.NavigateTo(Constants.Navigation.MainPage);
        }

        public async Task Save(StorageFile file)
        {
            var token = StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);
            await _mediator.Send(new SaveDatabaseCommand { FilePath = token });
            _navigation.NavigateTo(Constants.Navigation.MainPage);
        }

        public async Task Close()
        {
            await _mediator.Send(new CloseDatabaseCommand());
            _navigation.NavigateTo(Constants.Navigation.MainPage);
        }
    }
}