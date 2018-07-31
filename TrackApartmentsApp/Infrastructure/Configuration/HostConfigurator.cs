using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartmentsApp.Core.Contracts;
using TrackApartmentsApp.Core.Contracts.Storage;
using TrackApartmentsApp.Core.Interfaces.PageParser;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Data;
using TrackApartmentsApp.Data.PageParsers.Onliner;
using TrackApartmentsApp.Data.Storage;
using TrackApartmentsApp.Data.Storage.Abstract;
using TrackApartmentsApp.Domain.Connectors.Abstract;
using TrackApartmentsApp.Domain.Connectors.OnlinerConnector;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Secrets;
using TrackApartmentsApp.Domain.Sinks;
using TrackApartmentsApp.Domain.Sinks.Abstract;
using TrackApartmentsApp.Domain.Sinks.Conditions;

namespace TrackApartmentsApp.Infrastructure.Configuration
{
    public class HostConfigurator
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
                    services.AddScoped<ILoadEngine, LoadEngine>();

                    services.AddHttpClient<ILoadEngine, LoadEngine>();

                    services.AddScoped<IResponseParser, ResponseParser>();
                    services.AddScoped<IOnlinerPageParser, OnlinerPageParser>();
                    services.AddScoped<IOnlinerConnector, OnlinerConnector>();
                    services.AddScoped<IStorageWorker, StorageWorker>();
                    services.AddScoped<IStorageReadRepository<Apartment>, StorageAppartmentReadRepository>();
                    services.AddScoped<IStorageWriteRepository<Apartment>, StorageAppartmentWriteRepository>();
                    services.AddScoped<IStorageWorker, StorageWorker>();
                    services.AddScoped<IStorageConnector, OnlinerStorageConnector>();

                    services.AddScoped<IEmailCondition, EmailCondition>();
                    services.AddScoped<ISmsCondition, SmsCondition>();

                    services.AddScoped<ISink<Apartment>, SmsSink>();
                    services.AddScoped<ISink<Apartment>, EmailSink>();
                    services.AddScoped<ICompositeSink<Apartment>, CompositeSink>(ctx =>
                    {
                        var sinks = ctx.GetServices<ISink<Apartment>>();

                        var composite = new CompositeSink();

                        foreach (var service in sinks)
                        {
                            composite.Add((Sink)service);
                        }
                        return composite;
                    });

                    services.AddScoped<ApartmentService>();

                    services.AddSingleton(log);

                    ConfigureSettings(hostContext, services);
                });

            return builder.Build();
        }


        private static async Task LoadSecretSettings(StorageSettings storageSettings, AppSettings appSettings, TwilioSettings twilioSettings, SendGridSettings sendGridSettings)
        {
            var store = new SecretsStore(appSettings);

            storageSettings.ConnectionString = await store.GetOrLoadSettingAsync(storageSettings.ConnectionString);

            twilioSettings.AccountSid = await store.GetOrLoadSettingAsync(twilioSettings.AccountSid);
            twilioSettings.AuthToken = await store.GetOrLoadSettingAsync(twilioSettings.AuthToken);
            twilioSettings.PhoneNumberFrom = await store.GetOrLoadSettingAsync(twilioSettings.PhoneNumberFrom);
            twilioSettings.PhoneNumberTo = await store.GetOrLoadSettingAsync(twilioSettings.PhoneNumberTo);

            sendGridSettings.Password = await store.GetOrLoadSettingAsync(sendGridSettings.Password);
            sendGridSettings.UserName = await store.GetOrLoadSettingAsync(sendGridSettings.UserName);
        }

        private static void ConfigureSettings(HostBuilderContext hostContext, IServiceCollection services)
        {
            var onlinerSettings = new OnlinerSettings();
            var storageSettings = new StorageSettings();
            var appSettings = new AppSettings();
            var twilioSettings = new TwilioSettings();
            var sendGridSettings = new SendGridSettings();

            hostContext.Configuration.GetSection(nameof(StorageSettings)).Bind(storageSettings);
            hostContext.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
            hostContext.Configuration.GetSection(nameof(OnlinerSettings)).Bind(onlinerSettings);
            hostContext.Configuration.GetSection(nameof(TwilioSettings)).Bind(twilioSettings);
            hostContext.Configuration.GetSection(nameof(SendGridSettings)).Bind(sendGridSettings);

            LoadSecretSettings(storageSettings, appSettings, twilioSettings, sendGridSettings)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(storageSettings);
            services.AddSingleton(appSettings);
            services.AddSingleton(onlinerSettings);
            services.AddSingleton(twilioSettings);
            services.AddSingleton(sendGridSettings);
        }
    }
}
