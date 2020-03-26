using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Recent.Commands.AddRecent
{
    public class AddRecentCommand: IRequest
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public class AddRecentCommandHandler: IRequestHandler<AddRecentCommand>
        {
            private readonly IRecentProxy _recent;

            public AddRecentCommandHandler(IRecentProxy recent)
            {
                _recent = recent;
            }

            public void Handle(AddRecentCommand message)
            {
                _recent.Add(new FileInfo
                {
                    Name = message.Name,
                    Path = message.Path
                });
            }
        }
    }
}