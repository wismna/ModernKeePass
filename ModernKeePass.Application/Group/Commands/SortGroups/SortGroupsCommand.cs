using System.Collections.Generic;
using System.Linq;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.SortGroups
{
    public class SortGroupsCommand : IRequest
    {
        public GroupVm Group { get; set; }

        public class SortGroupsCommandHandler : IRequestHandler<SortGroupsCommand>
        {
            private readonly IDatabaseProxy _database;

            public SortGroupsCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SortGroupsCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                _database.SortSubGroups(message.Group.Id);
                message.Group.SubGroups = message.Group.SubGroups.OrderBy(g => g.Title).ToList();
            }
        }
    }
}