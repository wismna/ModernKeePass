using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Controls;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Views;

namespace ModernKeePass.ViewModels
{
    public class SaveVm
    {
        public bool IsSaveEnabled => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult().IsDirty;

        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }
        public Frame Frame { get; set; }

        private readonly IMediator _mediator;

        public SaveVm() : this(App.Services.GetRequiredService<IMediator>()) { }

        public SaveVm(IMediator mediator)
        {
            _mediator = mediator;
            SaveCommand = new RelayCommand(async () => await Save(), () => IsSaveEnabled);
            CloseCommand = new RelayCommand(async () => await Close());
        }

        public async Task Save(bool close = true)
        {
            await _mediator.Send(new SaveDatabaseCommand());
            if (close) await _mediator.Send(new CloseDatabaseCommand());
            Frame.Navigate(typeof(MainPage));
        }

        public async Task Save(StorageFile file)
        {
            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            await _mediator.Send(new SaveDatabaseCommand { FilePath = token });
            Frame.Navigate(typeof(MainPage));
        }

        public async Task Close()
        {
            await _mediator.Send(new CloseDatabaseCommand());
            Frame.Navigate(typeof(MainPage));
        }
    }
}