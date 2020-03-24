using FluentValidation;

namespace ModernKeePass.Application.Database.Queries.OpenDatabase
{
    public class OpenDatabaseQueryValidator : AbstractValidator<OpenDatabaseQuery>
    {
        public OpenDatabaseQueryValidator()
        {
            RuleFor(v => v.Credentials != null && v.FileInfo != null);
        }
    }
}