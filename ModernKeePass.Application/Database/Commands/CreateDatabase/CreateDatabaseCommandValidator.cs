using FluentValidation;

namespace ModernKeePass.Application.Database.Commands.CreateDatabase
{
    public class CreateDatabaseCommandValidator : AbstractValidator<CreateDatabaseCommand>
    {
        public CreateDatabaseCommandValidator()
        {
            RuleFor(v => v.FilePath)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.Password)
                .NotNull()
                .NotEmpty()
                .When(v => string.IsNullOrEmpty(v.KeyFilePath));
            RuleFor(v => v.KeyFilePath)
                .NotNull()
                .NotEmpty()
                .When(v => string.IsNullOrEmpty(v.Password));
        }
    }
}