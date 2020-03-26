using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.SortGroups
{
    public class SortGroupsCommand : IRequest
    {
        public GroupVm Group { get; set; }

        public class SortGroupsCommandHandler : IAsyncRequestHandler<SortGroupsCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public SortGroupsCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(SortGroupsCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                _database.SortSubGroups(message.Group.Id);
            }
        }
    }
}