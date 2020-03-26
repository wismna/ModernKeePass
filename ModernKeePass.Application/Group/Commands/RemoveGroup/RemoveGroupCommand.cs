using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.RemoveGroup
{
    public class RemoveGroupCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public GroupVm Group { get; set; }

        public class RemoveGroupCommandHandler : IAsyncRequestHandler<RemoveGroupCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public RemoveGroupCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(RemoveGroupCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                await _database.RemoveGroup(message.ParentGroup.Id, message.Group.Id);
                message.ParentGroup.SubGroups.Remove(message.Group);
            }
        }
    }
}