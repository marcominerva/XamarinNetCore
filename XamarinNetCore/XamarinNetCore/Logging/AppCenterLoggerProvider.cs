using Microsoft.Extensions.Logging;

namespace XamarinNetCore.Logging
{
    [ProviderAlias("AppCenter")]
    public class AppCenterLoggerProvider : ILoggerProvider
    {
        // Create an instance of an ILogger, which is used to actually write the logs
        public ILogger CreateLogger(string categoryName) => new AppCenterLogger(this, categoryName);

        public void Dispose()
        {
        }
    }
}
