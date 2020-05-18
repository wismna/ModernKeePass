using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using Microsoft.HockeyApp;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Log
{
    public class HockeyAppLog : ILogger
    {
        private readonly IHockeyClient _hockey;
        private readonly IFileProxy _file;
        private readonly IDateTime _dateTime;

        public HockeyAppLog(IHockeyClient hockey, IFileProxy file, IDateTime dateTime)
        {
            _hockey = hockey;
            _file = file;
            _dateTime = dateTime;
        }

        public async Task LogError(Exception exception)
        {
            _hockey.TrackException(exception);
            _hockey.Flush();

            var time = _dateTime.Now.ToLocalTime();
            var data = new List<string>
            {
                $"{time} - {exception.Message}",
                $"{time} - {exception.StackTrace}"
            };
            await _file.WriteToLogFile(data);
        }

        public void LogTrace(string message, Dictionary<string, string> values)
        {
            _hockey.TrackTrace(message, values);
            _hockey.Flush();
        }
    }
}
