using Microsoft.Extensions.Options;
using XamarinNetCore.Models;

namespace XamarinNetCore.Services
{
    public class FakeService : IService
    {
        private readonly AppSettings settings;

        public string Description => "This is a FAKE service, don't worry";

        public FakeService(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }
    }
}
