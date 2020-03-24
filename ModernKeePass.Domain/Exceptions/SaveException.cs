using System;

namespace ModernKeePass.Domain.Exceptions
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
