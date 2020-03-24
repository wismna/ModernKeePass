using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CreateDatabase
{
    public class CreateDatabaseCommand : IRequest<DatabaseVm>
    {
        public FileInfo FileInfo { get; set; }
        public Credentials Credentials { get; set; }

        public class CreateDatabaseCommandHandler : IRequestHandler<CreateDatabaseCommand, DatabaseVm>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public CreateDatabaseCommandHandler(IDatabaseProxy database, IMediator mediator, IMapper mapper)
            {
                _database = database;
                _mediator = mediator;
                _mapper = mapper;
            }

            public async Task<DatabaseVm> Handle(CreateDatabaseCommand message, CancellationToken cancellationToken)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery(), cancellationToken);
                if (isDatabaseOpen) throw new DatabaseOpenException();

                var database = await _database.Create(message.FileInfo, message.Credentials);
                var databaseVm = new DatabaseVm
                {
                    IsOpen = true,
                    Name = database.Name,
                    RootGroup = _mapper.Map<GroupVm>(database.RootGroupEntity)
                };
                return databaseVm;
            }
        }
    }
}