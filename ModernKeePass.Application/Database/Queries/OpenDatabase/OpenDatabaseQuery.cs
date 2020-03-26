using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Queries.OpenDatabase
{
    public class OpenDatabaseQuery: IRequest<GroupVm>
    {
        public string FilePath { get; set; }
        public string Password { get; set; }
        public string KeyFilePath { get; set; }

        public class OpenDatabaseQueryHandler : IAsyncRequestHandler<OpenDatabaseQuery, GroupVm>
        {
            private readonly IMapper _mapper;
            private readonly IDatabaseProxy _database;

            public OpenDatabaseQueryHandler(IMapper mapper, IDatabaseProxy database)
            {
                _mapper = mapper;
                _database = database;
            }

            public async Task<GroupVm> Handle(OpenDatabaseQuery request)
            {
                if (_database.IsOpen) throw new DatabaseOpenException();

                var rootGroup = await _database.Open(
                    new FileInfo
                    {
                        Path = request.FilePath
                    }, 
                    new Credentials
                    {
                        KeyFilePath = request.KeyFilePath,
                        Password = request.Password
                    });
                return _mapper.Map<GroupVm>(rootGroup);
            }
        }
    }
}