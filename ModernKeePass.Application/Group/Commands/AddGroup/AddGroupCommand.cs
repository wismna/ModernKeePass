using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.AddGroup
{
    public class AddGroupCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public GroupVm Group { get; set; }

        public class AddGroupCommandHandler : IAsyncRequestHandler<AddGroupCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public AddGroupCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(AddGroupCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                await _database.AddGroup(message.ParentGroup.Id, message.Group.Id);
                message.ParentGroup.SubGroups.Add(message.Group);
            }
        }
    }
}