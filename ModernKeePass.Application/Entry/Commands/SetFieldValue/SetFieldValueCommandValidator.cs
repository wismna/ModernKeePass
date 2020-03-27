using FluentValidation;

namespace ModernKeePass.Application.Entry.Commands.SetFieldValue
{
    public class SetFieldValueCommandValidator: AbstractValidator<SetFieldValueCommand>
    {
        public SetFieldValueCommandValidator()
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