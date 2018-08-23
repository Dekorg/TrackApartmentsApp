using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts;
using TrackApartments.Contracts.PageParser;
using TrackApartments.Core.Load;
using TrackApartments.Core.Secrets;
using TrackApartments.Onliner.Domain.Connector;
using TrackApartments.Onliner.Domain.PageParsers.Onliner;
using TrackApartments.Onliner.Settings;

namespace TrackApartments.Onliner.Infrastructure.Configuration
{
    public class OnlinerHostConfigurator
    {
        //Currently it's better to use static client that is shared between func instances to avoid socket exhaustion.
        public static readonly HttpClient Client = new HttpClient();

        public IHost BuildHost(ExecutionContext context, ILogger log)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(context.FunctionAppDirectory)
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();

                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddScoped<ILoadEngine, LoadEngine>();

                    services.AddScoped<ILoadEngine, LoadEngine>();
                    services.AddSingleton<HttpClient>(Client);

                    services.AddScoped<IResponseParser, ResponseParser>();
                    services.AddScoped<IOnlinerPageParser, OnlinerPageParser>();
                    services.AddScoped<IOnlinerConnector, OnlinerConnector>();
                    services.AddScoped<OnlinerApartmentService>();

                    services.AddSingleton(log);

                    ConfigureSettings(hostContext, services);
                });

            return builder.Build();
        }


        private static async Task LoadSecretSettings(QueueStorageSettings queueStorageSettings, AppSettings appSettings)
        {
            var store = new SecretsStore(appSettings.KeyVaultBaseUrl);
            queueStorageSettings.ConnectionString = await store.GetOrLoadSettingAsync(queueStorageSettings.ConnectionString);
        }

        private static void ConfigureSettings(HostBuilderContext hostContext, IServiceCollection services)
        {
            var storageSettings = new QueueStorageSettings();
            var appSettings = new AppSettings();

            hostContext.Configuration.GetSection(nameof(QueueStorageSettings)).Bind(storageSettings);
            hostContext.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);

            LoadSecretSettings(storageSettings, appSettings)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(storageSettings);
            services.AddSingleton(appSettings);
        }
    }
}
