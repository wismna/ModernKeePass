using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Group.Queries.GetGroup;
using ModernKeePass.Application.Parameters.Commands.SetHasRecycleBin;
using ModernKeePass.Application.Parameters.Commands.SetRecycleBin;

namespace ModernKeePass.ViewModels.Settings
{
    public class RecycleBinVm: ObservableObject
    {
        private readonly IMediator _mediator;
        private readonly DatabaseVm _database;

        public bool HasRecycleBin
        {
            get { return _database.IsRecycleBinEnabled; }
            set
            {
                _mediator.Send(new SetHasRecycleBinCommand { HasRecycleBin = value }).Wait();
                RaisePropertyChanged(nameof(HasRecycleBin));
            }
        }

        public bool IsNewRecycleBin
        {
            get { return string.IsNullOrEmpty(_database.RecycleBinId); }
            set
            {
                if (value) _mediator.Send(new SetRecycleBinCommand { RecycleBinId = null }).Wait();
            }
        }

        public ObservableCollection<IEntityVm> Groups { get; }

        public IEntityVm SelectedRecycleBin
        {
            get { return Groups.FirstOrDefault(g => g.Id == _database.RecycleBinId); }
            set
            {
                if (!IsNewRecycleBin) _mediator.Send(new SetRecycleBinCommand { RecycleBinId = value.Id }).Wait();
            }
        }

        public RecycleBinVm(IMediator mediator)
        {
            _mediator = mediator;
            _database = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            var rootGroup = _mediator.Send(new GetGroupQuery { Id = _database.RootGroupId }).GetAwaiter().GetResult();
            Groups = new ObservableCollection<IEntityVm>(rootGroup.SubGroups);
        }
    }
}