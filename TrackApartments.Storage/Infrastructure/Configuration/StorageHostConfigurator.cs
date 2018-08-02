using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts;
using TrackApartments.Contracts.Models;
using TrackApartments.Core.Secrets;
using TrackApartments.Storage.Domain.Contracts;
using TrackApartments.Storage.Domain.Storage;
using TrackApartments.Storage.Domain.Storage.Abstract;
using TrackApartments.Storage.Queue;
using TrackApartments.Storage.Settings;

namespace TrackApartments.Storage.Infrastructure.Configuration
{
    public class StorageHostConfigurator
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
                    services.AddScoped<IQueueWriter<Apartment>, QueueWriter>();
                    services.AddScoped<IStorageReadRepository<Apartment>, StorageAppartmentReadRepository>();
                    services.AddScoped<IStorageWriteRepository<Apartment>, StorageAppartmentWriteRepository>();
                    services.AddScoped<IStorageWorker, StorageWorker>();
                    services.AddScoped<IStorageConnector, StorageConnector>();
                    services.AddScoped<StorageApartmentService>();
                    services.AddSingleton(log);

                    ConfigureSettings(hostContext, services);
                });

            return builder.Build();
        }


        private static async Task LoadSecretSettings(QueueStorageSettings queueStorageSettings, StorageSettings storageSettings, AppSettings appSettings)
        {
            var store = new SecretsStore(appSettings.KeyVaultBaseUrl);
            storageSettings.ConnectionString = await store.GetOrLoadSettingAsync(storageSettings.ConnectionString);
            queueStorageSettings.ConnectionString = await store.GetOrLoadSettingAsync(queueStorageSettings.ConnectionString);
        }

        private static void ConfigureSettings(HostBuilderContext hostContext, IServiceCollection services)
        {
            var storageSettings = new StorageSettings();
            var queueStorageSettings = new QueueStorageSettings();

            var appSettings = new AppSettings();
            hostContext.Configuration.GetSection(nameof(StorageSettings)).Bind(storageSettings);
            hostContext.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
            hostContext.Configuration.GetSection(nameof(QueueStorageSettings)).Bind(queueStorageSettings);

            LoadSecretSettings(queueStorageSettings, storageSettings, appSettings)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(queueStorageSettings);
            services.AddSingleton(storageSettings);
            services.AddSingleton(appSettings);
        }
    }
}
