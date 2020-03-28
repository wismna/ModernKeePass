using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Parameters.Commands.SetRecycleBin
{
    public class SetRecycleBinCommand : IRequest
    {
        public GroupVm RecycleBin { get; set; }

        public class SetRecycleBinCommandHandler : IRequestHandler<SetRecycleBinCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetRecycleBinCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetRecycleBinCommand message)
            {
                if (_database.IsOpen) _database.SetRecycleBin(message.RecycleBin.Id);
                else throw new DatabaseClosedException();
            }
        }
    }
}