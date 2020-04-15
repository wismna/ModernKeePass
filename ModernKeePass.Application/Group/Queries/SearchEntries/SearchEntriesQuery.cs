using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Queries.SearchEntries
{
    public class SearchEntriesQuery : IRequest<IEnumerable<EntryVm>>
    {
        public string GroupId { get; set; }
        public string SearchText { get; set; }

        public class SearchEntriesQueryHandler : IRequestHandler<SearchEntriesQuery, IEnumerable<EntryVm>>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public SearchEntriesQueryHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public IEnumerable<EntryVm> Handle(SearchEntriesQuery message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                return _database.Search(message.GroupId, message.SearchText).Select(e => _mapper.Map<EntryVm>(e));
            }
        }
    }
}