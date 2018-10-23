using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts;
using TrackApartments.Core.Load;
using TrackApartments.Core.Secrets;
using TrackApartments.Kufar.Domain.Connector;
using TrackApartments.Kufar.Settings;

namespace TrackApartments.Kufar.Infrastructure.Configuration
{
    public class KufarHostConfigurator
    {
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
                    services.AddScoped<IKufarConnector, KufarConnector>();
                    services.AddScoped<KufarApartmentService>();

                    services.AddSingleton(log);


                    ConfigureSettings(hostContext, services);
                });

            return builder.Build();
        }

        private static async Task LoadSecretSettings(QueueStorageSettings queueStorageSettings, AppSettings appSettings, KeyVaultSettings keyVaultSettings)
        {
            var store = new SecretsStore(appSettings.KeyVaultBaseUrl, keyVaultSettings.ClientId, keyVaultSettings.ClientSecret);
            queueStorageSettings.ConnectionString = await store.GetOrLoadSettingAsync(queueStorageSettings.ConnectionString);
        }

        private static void ConfigureSettings(HostBuilderContext hostContext, IServiceCollection services)
        {
            var storageSettings = new QueueStorageSettings();
            var appSettings = new AppSettings();
            var keyVaultSettings = new KeyVaultSettings();

            hostContext.Configuration.GetSection(nameof(QueueStorageSettings)).Bind(storageSettings);
            hostContext.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
            hostContext.Configuration.GetSection(nameof(KeyVaultSettings)).Bind(keyVaultSettings);

            LoadSecretSettings(storageSettings, appSettings, keyVaultSettings)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(storageSettings);
            services.AddSingleton(appSettings);
        }
    }
}
