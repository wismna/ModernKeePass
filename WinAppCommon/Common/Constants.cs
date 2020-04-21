namespace ModernKeePass.Common
{
    public class Constants
    {
        public class Navigation
        {
            public static string MainPage => nameof(MainPage);
            public static string EntryPage => nameof(EntryPage);
            public static string GroupPage => nameof(GroupPage);
        }

        public class File
        {
            public static int OneMegaByte => 1048576;
        }

        public class Settings
        {
            public static string SaveSuspend => nameof(SaveSuspend);
            public static string Sample => nameof(Sample);
            public static string DefaultFileFormat => nameof(DefaultFileFormat);
        }
    }
}