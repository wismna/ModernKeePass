namespace ModernKeePass.Domain.Common
{
    public static class Constants
    {
        public static string EmptyId => "00000000000000000000000000000000";

        public static class Extensions
        {
            public static string Any => "*";
            public static string Kdbx => ".kdbx";
            public static string Key => ".key";
        }
    }
}