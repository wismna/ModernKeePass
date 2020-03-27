using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Parameters.Commands.SetCompression
{
    public class SetCompressionCommand : IRequest
    {
        public string Compression { get; set; }

        public class SetCompressionCommandHandler : IRequestHandler<SetCompressionCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetCompressionCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetCompressionCommand message)
            {
                if (_database.IsOpen) _database.Compression = message.Compression;
                else throw new DatabaseClosedException();
            }
        }
    }
}