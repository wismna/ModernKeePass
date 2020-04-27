using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;

namespace ModernKeePass.Application.Common.Behaviors
{
    public class DirtyStatusBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly List<string> _excludedCommands = new List<string>
            {nameof(SaveDatabaseCommand), nameof(CloseDatabaseCommand)};

        private readonly IDatabaseProxy _database;

        public DirtyStatusBehavior(IDatabaseProxy database)
        {
            _database = database;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            var queryName = typeof(TRequest).Name;
            if (queryName.Contains("Command"))
            {
                _database.IsDirty = !_excludedCommands.Contains(queryName);
            }

            return response;
        }

    }
}