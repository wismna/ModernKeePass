namespace ModernKeePass.Common
{
    public static class Constants
    {
        public static class Navigation
        {
            public static string MainPage => nameof(MainPage);
            public static string EntryPage => nameof(EntryPage);
            public static string GroupPage => nameof(GroupPage);
        }

        public static class File
        {
            public static int OneMegaByte => 1048576;
        }

        public static class Settings
        {
            public static string SaveSuspend => nameof(SaveSuspend);
            public static string Sample => nameof(Sample);
            public static string DefaultFileFormat => nameof(DefaultFileFormat);
            public static string ClipboardTimeout => nameof(ClipboardTimeout);
            public static string HistoryMaxCount => nameof(HistoryMaxCount);

            public static class PasswordGenerationOptions
            {
                public static string UpperCasePattern => nameof(UpperCasePattern);
                public static string LowerCasePattern => nameof(LowerCasePattern);
                public static string DigitsPattern => nameof(DigitsPattern);
                public static string MinusPattern => nameof(MinusPattern);
                public static string UnderscorePattern => nameof(UnderscorePattern);
                public static string SpacePattern => nameof(SpacePattern);
                public static string SpecialPattern => nameof(SpecialPattern);
                public static string BracketsPattern => nameof(BracketsPattern);
            }
        }
    }
}