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
        }
    }
}