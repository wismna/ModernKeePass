using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.AddHistory
{
    public class AddHistoryCommand : IRequest
    {
        public EntryVm Entry { get; set; }

        public class AddHistoryCommandHandler : IRequestHandler<AddHistoryCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public AddHistoryCommandHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public void Handle(AddHistoryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var history = _database.AddHistory(message.Entry.Id);
                message.Entry.History.Add(_mapper.Map<EntryVm>(history));
            }
        }
    }
}