using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using MediatR;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Application.Group.Queries.GetAllGroups;
using ModernKeePass.Application.Group.Queries.SearchEntries;
using ModernKeePass.Common;
using ModernKeePass.Models;

namespace ModernKeePass.ViewModels
{
    public class TopMenuVm
    {
        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        private readonly DatabaseVm _database;
        public IEnumerable<GroupVm> Groups { get; set; }

        public TopMenuVm(IMediator mediator, INavigationService navigation)
        {
            _mediator = mediator;
            _navigation = navigation;

            _database = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            Groups = _mediator.Send(new GetAllGroupsQuery { GroupId = _database.RootGroupId }).GetAwaiter().GetResult();
        }

        public void GoToEntry(string entryId, bool isNew = false)
        {
            _navigation.NavigateTo(Constants.Navigation.EntryPage, new NavigationItem
            {
                Id = entryId,
                IsNew = isNew
            });
        }

        public async Task<IEnumerable<EntryVm>> Search(string queryText)
        {
            return await _mediator.Send(new SearchEntriesQuery { GroupId = _database.RootGroupId, SearchText = queryText });
        }
    }
}