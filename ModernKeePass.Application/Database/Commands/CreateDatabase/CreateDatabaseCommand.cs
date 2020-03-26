using MediatR;
using System.Threading.Tasks;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CreateDatabase
{
    public class CreateDatabaseCommand : IRequest<GroupVm>
    {
        public FileInfo FileInfo { get; set; }
        public Credentials Credentials { get; set; }

        public class CreateDatabaseCommandHandler : IAsyncRequestHandler<CreateDatabaseCommand, GroupVm>
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

            public async Task<GroupVm> Handle(CreateDatabaseCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (isDatabaseOpen) throw new DatabaseOpenException();

                var rootGroup = await _database.Create(message.FileInfo, message.Credentials);
                return _mapper.Map<GroupVm>(rootGroup);
            }

        }
    }
}