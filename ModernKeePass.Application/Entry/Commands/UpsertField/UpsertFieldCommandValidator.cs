using FluentValidation;

namespace ModernKeePass.Application.Entry.Commands.UpsertField
{
    public class UpsertFieldCommandValidator: AbstractValidator<UpsertFieldCommand>
    {
        public UpsertFieldCommandValidator()
        {
            RuleFor(v => v.EntryId)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.FieldName)
                .NotNull()
                .NotEmpty();
        }
    }
}