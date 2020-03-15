using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Xamarin.Forms;
using XamarinNetCore.Services;

namespace XamarinNetCore
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly IService service;
        private readonly ILogger<MainPage> logger;

        public MainPage(IService service, ILogger<MainPage> logger)
        {
            InitializeComponent();

            this.service = service;
            this.logger = logger;
        }

        protected override void OnAppearing()
        {
            logger.LogInformation("Caricamento della pagina");
            base.OnAppearing();
        }
    }
}
