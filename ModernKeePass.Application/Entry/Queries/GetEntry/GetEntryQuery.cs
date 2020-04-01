using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Queries.GetEntry
{
    public class GetEntryQuery: IRequest<EntryVm>
    {
        public string Id { get; set; }

        public class GetEntryQueryHandler: IRequestHandler<GetEntryQuery, EntryVm>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public GetEntryQueryHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public EntryVm Handle(GetEntryQuery message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                return _mapper.Map<EntryVm>(_database.GetEntry(message.Id));
            }
        }
    }
}