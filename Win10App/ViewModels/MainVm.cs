using Windows.Storage;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.GetDatabase;

namespace ModernKeePass.ViewModels
{
    public class MainVm
    {
        public bool IsDatabaseOpened { get; }
        public bool HasRecentItems { get; }

        public string OpenedDatabaseName { get; }
        public IStorageFile File { get; set; }

        public MainVm(IMediator mediator, IRecentProxy recent)
        {
            var database = mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            IsDatabaseOpened = database.IsOpen;
            OpenedDatabaseName = database.Name;
            HasRecentItems = recent.EntryCount > 0;
        }
    }
}