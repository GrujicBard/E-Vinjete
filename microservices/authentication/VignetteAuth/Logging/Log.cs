using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VignetteAuth.Logging
{
    public class Log
    {
        public Guid CorrelationId { get; } = Guid.NewGuid();
        public DateTimeOffset Timestamp { get; } = DateTime.UtcNow;
        public LogType LogType { get; set; }
        public string Url { get; } = "https://localhost:44372";
        public string AppName { get; } = "VignetteAuth";
        public string HttpCall { get; set; }

        public Log(LogType logType, string httpCall)
        {
            LogType = logType;
            HttpCall = httpCall;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} Correlation: {3} [{4}] - <* {5} *>", Timestamp, LogType, Url, CorrelationId, AppName, HttpCall);
        }
    }
}
