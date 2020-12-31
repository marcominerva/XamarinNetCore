using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Net.Http;
using Xamarin.Forms;
using XamarinNetCore.Services;

namespace XamarinNetCore
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly IService service;
        private readonly ILogger<MainPage> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public MainPage(IService service, ILogger<MainPage> logger, IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();

            this.service = service;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        protected override async void OnAppearing()
        {
            logger.LogInformation("Page appearing");

            //try
            //{
            //    var a = 0;
            //    var b = 5 / a;
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError(ex, "Error while opening the page");
            //}

            var client = httpClientFactory.CreateClient("openweathermap");
            var weatherResponse = await client.GetAsync("weather?zip=18018,IT&units=metric&APPID={OPENWEATHERMAP_APP_ID}");

            if (weatherResponse.IsSuccessStatusCode)
            {
                var weather = await weatherResponse.Content.ReadAsStringAsync();
                logger.LogInformation(weather);
            }

            base.OnAppearing();
        }
    }
}
