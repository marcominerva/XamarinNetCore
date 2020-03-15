using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Reflection;
using Xamarin.Essentials;
using XamarinNetCore.Logging;
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
                            logging.AddProvider(new AppCenterLoggerProvider());
                        })
                        .Build();

            // Save our service provider so we can use it later.
            ServiceProvider = host.Services;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var appSettingsSection = context.Configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettingsSection);

            services.AddHttpClient("openweathermap", client =>
            {
                client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                // The AddTransientHttpErrorPolicy handles errors typical of Http calls:
                // Network failures (System.Net.Http.HttpRequestException)
                // HTTP 5XX status codes (server errors)
                // HTTP 408 status code (request timeout)
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            }));

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
