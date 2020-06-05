using FluentValidation;

namespace ModernKeePass.Application.Import.Commands.ImportFromCsv
{
    public class ImportFromCsvCommandValidator : AbstractValidator<ImportFromCsvCommand>
    {
        public ImportFromCsvCommandValidator()
        {
            RuleFor(v => v.FilePath)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.DestinationGroupId)
                .NotNull()
                .NotEmpty();
        }
    }
}