using System;

namespace ModernKeePass.Exceptions
{
    public class SaveException : Exception
    {
        public new Exception InnerException { get; }

        public SaveException(Exception exception)
        {
            InnerException = exception;
        }
    }
}
