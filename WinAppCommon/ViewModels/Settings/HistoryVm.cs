using MediatR;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Parameters.Commands.SetMaxHistoryCount;
using ModernKeePass.Application.Parameters.Commands.SetMaxHistorySize;

namespace ModernKeePass.ViewModels.Settings
{
    public class HistoryVm
    {
        private readonly IMediator _mediator;
        private readonly DatabaseVm _database;
        
        public int MaxCount
        {
            get { return _database.MaxHistoryCount; }
            set { _mediator.Send(new SetMaxHistoryCountCommand { MaxHistoryCount = value }).Wait(); }
        }

        public long MaxSize
        {
            get { return _database.MaxHistorySize / 1024 / 1024; }
            set { _mediator.Send(new SetMaxHistorySizeCommand { MaxHistorySize = value * 1024 * 1024 }).Wait(); }
        }

        public HistoryVm(IMediator mediator)
        {
            _mediator = mediator;
            _database = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
        }
    }
}