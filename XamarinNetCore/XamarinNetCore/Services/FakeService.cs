using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XamarinNetCore.Models;

namespace XamarinNetCore.Services
{
    public class FakeService : IService
    {
        private readonly AppSettings settings;
        private readonly ILogger<FakeService> logger;

        public string Description => "This is a FAKE service, don't worry";

        public FakeService(IOptions<AppSettings> settings, ILogger<FakeService> logger)
        {
            this.settings = settings.Value;

            this.logger = logger;
            logger.LogInformation("FakeService created");
        }
    }
}
