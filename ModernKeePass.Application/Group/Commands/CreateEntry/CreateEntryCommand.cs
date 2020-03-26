using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.CreateEntry
{
    public class CreateEntryCommand : IRequest<EntryVm>
    {
        public GroupVm ParentGroup { get; set; }

        public class CreateEntryCommandHandler : IRequestHandler<CreateEntryCommand, EntryVm>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public CreateEntryCommandHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public EntryVm Handle(CreateEntryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var entry = _database.CreateEntry(message.ParentGroup.Id);
                var entryVm = _mapper.Map<EntryVm>(entry);
                message.ParentGroup.Entries.Add(entryVm);
                return entryVm;
            }
        }
    }
}