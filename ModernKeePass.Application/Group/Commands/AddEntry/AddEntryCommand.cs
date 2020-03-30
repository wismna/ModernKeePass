﻿using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.AddEntry
{
    public class AddEntryCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public EntryVm Entry { get; set; }

        public class AddEntryCommandHandler : IAsyncRequestHandler<AddEntryCommand>
        {
            private readonly IDatabaseProxy _database;

            public AddEntryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(AddEntryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                /*var entryEntity = new EntryEntity
                {
                    Id = message.Entry.Id,
                    Name = message.Entry.Title,
                    UserName = message.Entry.Username,
                    Password = message.Entry.Password,
                    Url = message.Entry.Url,
                    Notes = message.Entry.Notes,
                    HasExpirationDate = message.Entry.HasExpirationDate,
                    ExpirationDate = message.Entry.ExpirationDate,
                    LastModificationDate = message.Entry.ModificationDate,
                    BackgroundColor = message.Entry.BackgroundColor,
                    ForegroundColor = message.Entry.ForegroundColor,
                    Icon = message.Entry.Icon,
                    AdditionalFields = message.Entry.AdditionalFields,
                    History = message.Entry.History
                };*/

                await _database.AddEntry(message.ParentGroup.Id, message.Entry.Id);
                message.ParentGroup.Entries.Add(message.Entry);
            }
        }
    }
}