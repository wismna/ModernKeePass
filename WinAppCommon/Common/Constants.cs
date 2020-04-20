namespace ModernKeePass.Common
{
    public class Constants
    {
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