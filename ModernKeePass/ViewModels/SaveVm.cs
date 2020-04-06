using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;

namespace ModernKeePass.ViewModels
{
    public class SaveVm
    {
        private readonly IMediator _mediator;
        public SaveVm() : this(App.Services.GetService<IMediator>()) { }

        public SaveVm(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Save(bool close = true)
        {
            await _mediator.Send(new SaveDatabaseCommand());
            if (close) await _mediator.Send(new CloseDatabaseCommand());
        }

        public async Task Save(StorageFile file)
        {
            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            await _mediator.Send(new SaveDatabaseCommand { FilePath = token });
        }
    }
}