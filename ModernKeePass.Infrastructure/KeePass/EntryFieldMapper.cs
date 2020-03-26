using ModernKeePass.Domain.Enums;
using ModernKeePassLib;

namespace ModernKeePass.Infrastructure.KeePass
{
    public static class EntryFieldMapper
    {
        public static string MapPwDefsToField(string value)
        {
            switch (value)
            {
                case PwDefs.TitleField: return EntryFieldName.Title;
                case PwDefs.UserNameField: return EntryFieldName.UserName;
                case PwDefs.PasswordField: return EntryFieldName.Password;
                case PwDefs.NotesField: return EntryFieldName.Notes;
                case PwDefs.UrlField: return EntryFieldName.Url;
                default: return value;
            }
        }
        public static string MapFieldToPwDef(string value)
        {
            switch (value)
            {
                case EntryFieldName.Title: return PwDefs.TitleField;
                case EntryFieldName.UserName: return PwDefs.UserNameField;
                case EntryFieldName.Password: return PwDefs.PasswordField;
                case EntryFieldName.Notes: return PwDefs.NotesField;
                case EntryFieldName.Url: return PwDefs.UrlField;
                default: return value;
            }
        }
    }
}