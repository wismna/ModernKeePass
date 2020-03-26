using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.CreateEntry
{
    public class CreateEntryCommand : IRequest<EntryVm>
    {
        public GroupVm ParentGroup { get; set; }

        public class CreateEntryCommandHandler : IAsyncRequestHandler<CreateEntryCommand, EntryVm>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public CreateEntryCommandHandler(IDatabaseProxy database, IMediator mediator, IMapper mapper)
            {
                _database = database;
                _mediator = mediator;
                _mapper = mapper;
            }

            public async Task<EntryVm> Handle(CreateEntryCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                var entry = _database.CreateEntry(message.ParentGroup.Id);
                var entryVm = _mapper.Map<EntryVm>(entry);
                message.ParentGroup.Entries.Add(entryVm);
                return entryVm;
            }
        }
    }
}