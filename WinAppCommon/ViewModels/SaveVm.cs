using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.ViewModels
{
    public class SaveVm: ViewModelBase
    {
        public bool IsSaveEnabled => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult().IsDirty;

        public RelayCommand SaveAsCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }

        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        private readonly IFileProxy _file;

        public SaveVm(IMediator mediator, INavigationService navigation, IFileProxy file)
        {
            _mediator = mediator;
            _navigation = navigation;
            _file = file;

            SaveAsCommand = new RelayCommand(async () => await SaveAs());
            SaveCommand = new RelayCommand(async () => await Save(), () => IsSaveEnabled);
            CloseCommand = new RelayCommand(async () => await Close());
        }
        
        private async Task SaveAs()
        {
            // TODO: get these from resource
            var file = await _file.CreateFile("New Database", Domain.Common.Constants.Extensions.Kdbx, "KeePass 2.x database", true);

            await _mediator.Send(new SaveDatabaseCommand { FilePath = file.Id });
            _navigation.NavigateTo(Constants.Navigation.MainPage);
        }

        public async Task Save()
        {
            try
            {
                await _mediator.Send(new SaveDatabaseCommand());
                await Close();
            }
            catch (SaveException e)
            {
                MessengerInstance.Send(new SaveErrorMessage { Message = e.Message });
            }
        }
        
        public async Task Close()
        {
            await _mediator.Send(new CloseDatabaseCommand());
            _navigation.NavigateTo(Constants.Navigation.MainPage);
        }
    }
}