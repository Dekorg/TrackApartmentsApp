using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Core.Secrets;
using TrackApartments.Data.Abstract;
using TrackApartments.Data.Contracts;
using TrackApartments.Data.Contracts.Settings;
using TrackApartments.Data.Repository;
using TrackApartments.Get.Domain;
using TrackApartments.Get.Domain.Contracts;
using TrackApartments.Get.Settings;

namespace TrackApartments.Get.Infrastructure.Configuration
{
    public class GetApartmentHostConfigurator
    {
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
                    services.AddScoped<IStorageWorker, StorageWorker>();
                    services.AddScoped<IStorageReadRepository<Apartment>, StorageApartmentReadRepository>();
                    services.AddScoped<IStorageConnector, StorageConnector>();
                    services.AddScoped<GetApartmentService>();
                    services.AddSingleton(log);

                    ConfigureSettings(hostContext, services);
                });

            return builder.Build();
        }

        private static async Task LoadSecretSettings(
            StorageSettings storageSettings,
            AppSettings appSettings,
            KeyVaultSettings keyVaultSettings)
        {
            var store = new SecretsStore(appSettings.KeyVaultBaseUrl, keyVaultSettings.ClientId, keyVaultSettings.ClientSecret);
            storageSettings.ConnectionString = await store.GetOrLoadSettingAsync(storageSettings.ConnectionString);
        }

        private static void ConfigureSettings(HostBuilderContext hostContext, IServiceCollection services)
        {
            var storageSettings = new StorageSettings();
            var appSettings = new AppSettings();
            var keyVaultSettings = new KeyVaultSettings();

            hostContext.Configuration.GetSection(nameof(StorageSettings)).Bind(storageSettings);
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
