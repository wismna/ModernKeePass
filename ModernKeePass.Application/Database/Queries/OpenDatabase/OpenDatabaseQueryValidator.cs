﻿using FluentValidation;

namespace ModernKeePass.Application.Database.Queries.OpenDatabase
{
    public class OpenDatabaseQueryValidator : AbstractValidator<OpenDatabaseQuery>
    {
        public OpenDatabaseQueryValidator()
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