using System;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.AddAttachment
{
    public class AddAttachmentCommand : IRequest
    {
        public EntryVm Entry { get; set; }
        public string AttachmentName { get; set; }
        public byte[] AttachmentContent { get; set; }

        public class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand>
        {
            private readonly IDatabaseProxy _database;

            public AddAttachmentCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(AddAttachmentCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                if (message.Entry.Attachments.ContainsKey(message.AttachmentName)) throw new ArgumentException("AttachmentAlreadyExists", nameof(message.AttachmentName));
                _database.AddAttachment(message.Entry.Id, message.AttachmentName, message.AttachmentContent);
                message.Entry.Attachments.Add(message.AttachmentName, message.AttachmentContent);
            }
        }
    }
}