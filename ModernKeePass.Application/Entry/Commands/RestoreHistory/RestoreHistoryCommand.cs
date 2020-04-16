using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.RestoreHistory
{
    public class RestoreHistoryCommand : IRequest
    {
        public EntryVm Entry { get; set; }
        public int HistoryIndex { get; set; }

        public class RestoreHistoryCommandHandler : IRequestHandler<RestoreHistoryCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public RestoreHistoryCommandHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public void Handle(RestoreHistoryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var entry = _database.RestoreFromHistory(message.Entry.Id, message.HistoryIndex);
                message.Entry = _mapper.Map<EntryVm>(entry);
            }
        }
    }
}