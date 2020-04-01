using MediatR;
using System.Threading.Tasks;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CreateDatabase
{
    public class CreateDatabaseCommand : IRequest<GroupVm>
    {
        public string FilePath { get; set; }
        public string Password { get; set; }
        public string KeyFilePath { get; set; }

        public class CreateDatabaseCommandHandler : IAsyncRequestHandler<CreateDatabaseCommand, GroupVm>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public CreateDatabaseCommandHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public async Task<GroupVm> Handle(CreateDatabaseCommand message)
            {
                if (_database.IsOpen) throw new DatabaseOpenException();

                var rootGroup = await _database.Create(
                    new FileInfo
                    {
                        Path = message.FilePath
                    },
                    new Credentials
                    {
                        KeyFilePath = message.KeyFilePath,
                        Password = message.Password
                    });
                return GroupVm.BuildHierarchy(null, rootGroup, _mapper);
            }

        }
    }
}