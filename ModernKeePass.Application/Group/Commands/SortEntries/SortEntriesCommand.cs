using System.Linq;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.SortEntries
{
    public class SortEntriesCommand : IRequest
    {
        public GroupVm Group { get; set; }

        public class SortEntriesCommandHandler : IRequestHandler<SortEntriesCommand>
        {
            private readonly IDatabaseProxy _database;

            public SortEntriesCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SortEntriesCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                _database.SortEntries(message.Group.Id);
                message.Group.Entries = message.Group.Entries.OrderBy(e => e.Title).ToList();
            }
        }
    }
}