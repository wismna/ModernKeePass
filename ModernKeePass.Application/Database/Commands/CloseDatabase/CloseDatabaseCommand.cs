using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CloseDatabase
{
    public class CloseDatabaseCommand: IRequest
    {
        public class CloseDatabaseCommandHandler : IRequestHandler<CloseDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IFileProxy _file;

            public CloseDatabaseCommandHandler(IDatabaseProxy database, IFileProxy file)
            {
                _database = database;
                _file = file;
            }
            public void Handle(CloseDatabaseCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                _database.CloseDatabase();
                _file.ReleaseFile(_database.FileAccessToken);
                _database.FileAccessToken = null;
            }
        }
    }
}