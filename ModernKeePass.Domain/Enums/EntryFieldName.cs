using System.Collections.Generic;

namespace ModernKeePass.Domain.Enums
{
    public static class EntryFieldName
    {
        public const string Title = nameof(Title);
        public const string UserName = nameof(UserName);
        public const string Password = nameof(Password);
        public const string Url = "URL";
        public const string Notes = nameof(Notes);
        public const string Icon = nameof(Icon);
        public const string ExpirationDate = nameof(ExpirationDate);
        public const string HasExpirationDate = nameof(HasExpirationDate);
        public const string BackgroundColor = nameof(BackgroundColor);
        public const string ForegroundColor = nameof(ForegroundColor);

        public static IEnumerable<string> StandardFieldNames => new[]
        {
            Title,
            UserName,
            Password,
            Url,
            Notes,
            Icon,
            ExpirationDate,
            HasExpirationDate,
            BackgroundColor,
            ForegroundColor
        };
    }
}