using System.Collections.Generic;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.DeleteAttachment
{
    public class DeleteAttachmentCommand : IRequest
    {
        public EntryVm Entry { get; set; }
        public string AttachmentName { get; set; }

        public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand>
        {
            private readonly IDatabaseProxy _database;

            public DeleteAttachmentCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(DeleteAttachmentCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                if (!message.Entry.Attachments.ContainsKey(message.AttachmentName)) throw new KeyNotFoundException("AttachmentDoesntExist");
                _database.DeleteAttachment(message.Entry.Id, message.AttachmentName);
                message.Entry.Attachments.Remove(message.AttachmentName);
            }
        }
    }
}