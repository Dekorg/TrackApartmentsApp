using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackApartmentsApp.Core.Interfaces;
using TrackApartmentsApp.Core.Interfaces.PageParser;
using TrackApartmentsApp.Core.Interfaces.Storage;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Data;
using TrackApartmentsApp.Data.PageParsers.Onliner;
using TrackApartmentsApp.Data.Storage;
using TrackApartmentsApp.Data.Storage.Abstract;
using TrackApartmentsApp.Domain.Connectors.Abstract;
using TrackApartmentsApp.Domain.Connectors.OnlinerConnector;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Secrets;

namespace TrackApartmentsApp.Infrastructure.Configuration
{
    public class DependencyConfigurator
    {
        public IHost BuildHost(ExecutionContext context)
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

                    services.AddScoped<IResponseParser, ResponseParser>();
                    services.AddScoped<IOnlinerPageParser, OnlinerPageParser>();
                    services.AddScoped<IOnlinerConnector, OnlinerConnector>();

                    services.AddScoped<IStorageWorker, StorageWorker>();
                    services.AddScoped<IStorageReadRepository<Apartment>, StorageAppartmentReadRepository>();
                    services.AddScoped<IStorageWriteRepository<Apartment>, StorageAppartmentWriteRepository>();
                    services.AddScoped<IStorageWorker, StorageWorker>();

                    services.AddScoped<IStorageConnector, OnlinerStorageConnector>();

                    services.AddScoped<ApartmentService>();

                    ConfigureSettings(hostContext, services);
                });

            return builder.Build();
        }


        private static async Task LoadSecretSettings(StorageSettings storageSettings, AppSettings appSettings, TwilioSettings twilioSettings)
        {
            var store = new SecretsStore(appSettings);

            storageSettings.ConnectionString = await store.GetOrLoadSettingAsync(storageSettings.ConnectionString);
            twilioSettings.AccountSid = await store.GetOrLoadSettingAsync(twilioSettings.AccountSid);
            twilioSettings.AuthToken = await store.GetOrLoadSettingAsync(twilioSettings.AuthToken);
            twilioSettings.PhoneNumberFrom = await store.GetOrLoadSettingAsync(twilioSettings.PhoneNumberFrom);
            twilioSettings.PhoneNumberTo = await store.GetOrLoadSettingAsync(twilioSettings.PhoneNumberTo);
        }

        private static void ConfigureSettings(HostBuilderContext hostContext, IServiceCollection services)
        {
            var onlinerSettings = new OnlinerSettings();
            var storageSettings = new StorageSettings();
            var appSettings = new AppSettings();
            var twilioSettings = new TwilioSettings();

            hostContext.Configuration.GetSection(nameof(StorageSettings)).Bind(storageSettings);
            hostContext.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
            hostContext.Configuration.GetSection(nameof(OnlinerSettings)).Bind(onlinerSettings);
            hostContext.Configuration.GetSection(nameof(TwilioSettings)).Bind(twilioSettings);

            LoadSecretSettings(storageSettings, appSettings, twilioSettings).GetAwaiter().GetResult();

            services.AddSingleton(storageSettings);
            services.AddSingleton(appSettings);
            services.AddSingleton(onlinerSettings);
            services.AddSingleton(twilioSettings);
        }
    }
}
