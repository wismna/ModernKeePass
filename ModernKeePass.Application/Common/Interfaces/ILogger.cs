using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface ILogger
    {
        Task LogError(Exception exception);
        void LogTrace(string message, Dictionary<string, string> values);
    }
}