﻿using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Queries.OpenDatabase
{
    public class OpenDatabaseQuery: IRequest<GroupVm>
    {
        public FileInfo FileInfo { get; set; }
        public Credentials Credentials { get; set; }

        public class OpenDatabaseQueryHandler : IAsyncRequestHandler<OpenDatabaseQuery, GroupVm>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly IDatabaseProxy _databaseProxy;

            public OpenDatabaseQueryHandler(IMapper mapper, IMediator mediator, IDatabaseProxy databaseProxy)
            {
                _mapper = mapper;
                _mediator = mediator;
                _databaseProxy = databaseProxy;
            }

            public async Task<GroupVm> Handle(OpenDatabaseQuery request)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (isDatabaseOpen) throw new DatabaseOpenException();

                var rootGroup = await _databaseProxy.Open(request.FileInfo, request.Credentials);
                return _mapper.Map<GroupVm>(rootGroup);
            }
        }
    }
}