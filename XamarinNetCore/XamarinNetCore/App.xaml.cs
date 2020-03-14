using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace XamarinNetCore
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Host.Init();

            MainPage = Host.ServiceProvider.GetRequiredService<MainPage>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
