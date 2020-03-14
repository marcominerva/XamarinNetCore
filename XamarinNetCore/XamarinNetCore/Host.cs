using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using Xamarin.Essentials;

namespace XamarinNetCore
{
    public static class Host
    {
        public static IServiceProvider ServiceProvider { get; set; }

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
                        .ConfigureLogging((context, services) =>
                        {
                        })
                        .Build();

            //Save our service provider so we can use it later.
            ServiceProvider = host.Services;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddTransient<MainPage>();
        }
    }
}
