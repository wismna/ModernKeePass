using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Security.Commands.GeneratePassword
{
    public class GeneratePasswordCommand: IRequest<string>
    {
        public int PasswordLength { get; set; }
        public bool UpperCasePatternSelected { get; set; }
        public bool LowerCasePatternSelected { get; set; }
        public bool DigitsPatternSelected { get; set; }
        public bool SpecialPatternSelected { get; set; }
        public bool MinusPatternSelected { get; set; }
        public bool UnderscorePatternSelected { get; set; }
        public bool SpacePatternSelected { get; set; }
        public bool BracketsPatternSelected { get; set; }
        public string CustomChars { get; set; }

        public class GeneratePasswordCommandHandler: IRequestHandler<GeneratePasswordCommand, string>
        {
            private readonly ICredentialsProxy _security;

            public GeneratePasswordCommandHandler(ICredentialsProxy security)
            {
                _security = security;
            }

            public string Handle(GeneratePasswordCommand message)
            {
                var options = new PasswordGenerationOptions
                {
                    PasswordLength = message.PasswordLength,
                    BracketsPatternSelected = message.BracketsPatternSelected,
                    CustomChars = message.CustomChars,
                    DigitsPatternSelected = message.DigitsPatternSelected,
                    LowerCasePatternSelected = message.LowerCasePatternSelected,
                    MinusPatternSelected = message.MinusPatternSelected,
                    SpacePatternSelected = message.SpacePatternSelected,
                    SpecialPatternSelected = message.SpecialPatternSelected,
                    UnderscorePatternSelected = message.UnderscorePatternSelected,
                    UpperCasePatternSelected = message.UpperCasePatternSelected
                };
                return _security.GeneratePassword(options);
            }
        }
    }
}