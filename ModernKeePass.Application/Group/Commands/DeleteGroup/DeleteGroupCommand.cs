using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.DeleteGroup
{
    public class DeleteGroupCommand : IRequest
    {
        public GroupVm Group { get; set; }

        public class DeleteGroupCommandHandler : IAsyncRequestHandler<DeleteGroupCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public DeleteGroupCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(DeleteGroupCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                await _database.DeleteGroup(message.Group.Id);
                message.Group = null;
            }
        }
    }
}