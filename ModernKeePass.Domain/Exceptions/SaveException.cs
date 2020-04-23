using System;

namespace ModernKeePass.Domain.Exceptions
{
    public class SaveException : Exception
    {
        public new string Message { get; }
        public new string Source { get; }

        public SaveException(Exception exception)
        {
            Message = exception.Message;
            Source = exception.Source;
        }
    }
}
