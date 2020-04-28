using System.Collections.Generic;
using MediatR;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Application.Group.Queries.GetAllGroups;

namespace ModernKeePass.ViewModels
{
    public class TopMenuVm
    {
        public IEnumerable<GroupVm> Groups { get; set; }

        public TopMenuVm(IMediator mediator)
        {
            var database = mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            Groups = mediator.Send(new GetAllGroupsQuery { GroupId = database.RootGroupId }).GetAwaiter().GetResult();
        }
    }
}