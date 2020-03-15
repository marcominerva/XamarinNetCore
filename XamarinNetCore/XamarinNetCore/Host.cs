using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using Xamarin.Essentials;
using XamarinNetCore.Models;
using XamarinNetCore.Services;

namespace XamarinNetCore
{
    public static class Host
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("XamarinNetCore.appsettings.json");

            var host = new HostBuilder()
                        .ConfigureHostConfiguration(config =>
                        {
                            // Tell the host configuration where to find the files (this is required for Xamarin apps).
                            config.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

                            // Read in the configuration file.
                            config.AddJsonStream(stream);
                        })
                        .ConfigureServices((context, services) =>
                        {
                            // Configure our local services and access the host configuration.
                            ConfigureServices(context, services);
                        })
                        .ConfigureLogging(logging =>
                        {
                            logging.AddDebug();
                        })
                        .Build();

            // Save our service provider so we can use it later.
            ServiceProvider = host.Services;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var appSettingsSection = context.Configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettingsSection);

            if (context.HostingEnvironment.IsDevelopment())
            {
                services.AddScoped<IService, FakeService>();
            }
            else
            {
                services.AddScoped<IService, RealService>();
            }

            services.AddTransient<MainPage>();
        }
    }
}
