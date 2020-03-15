using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace XamarinNetCore.Logging
{
    public class AppCenterLogger : ILogger
    {
        private readonly AppCenterLoggerProvider provider;
        private readonly string category;

        public AppCenterLogger(AppCenterLoggerProvider loggerProvider, string categoryName)
        {
            provider = loggerProvider;
            category = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (logLevel >= LogLevel.Error && exception != null)
            {
                Crashes.TrackError(exception);
            }
            else
            {
                Analytics.TrackEvent(formatter(state, exception), new Dictionary<string, string>
                {
                    ["level"] = logLevel.ToString(),
                    ["category"] = category
                });
            }
        }
    }
}
