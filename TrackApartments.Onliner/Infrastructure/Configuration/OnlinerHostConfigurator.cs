using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts;
using TrackApartments.Contracts.Models;
using TrackApartments.Contracts.PageParser;
using TrackApartments.Core.Load;
using TrackApartments.Core.Secrets;
using TrackApartments.Onliner.Domain.Connector;
using TrackApartments.Onliner.Domain.PageParsers.Onliner;
using TrackApartments.Onliner.Domain.Queue;
using TrackApartments.Onliner.Settings;

namespace TrackApartments.Onliner.Infrastructure.Configuration
{
    public class OnlinerHostConfigurator
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
                    services.AddScoped<IQueueWriter<Apartment>, QueueWriter>();

                    //services.AddScoped<IStorageWorker, StorageWorker>();
                    //services.AddScoped<IStorageReadRepository<Apartment>, StorageAppartmentReadRepository>();
                    //services.AddScoped<IStorageWriteRepository<Apartment>, StorageAppartmentWriteRepository>();
                    //services.AddScoped<IStorageWorker, StorageWorker>();
                    //services.AddScoped<IStorageConnector, StorageConnector>();

                    //services.AddScoped<IEmailCondition, EmailCondition>();
                    //services.AddScoped<ISmsCondition, SmsCondition>();

                    //services.AddScoped<ISink<Apartment>, SmsSink>();
                    //services.AddScoped<ISink<Apartment>, EmailSink>();
                    //services.AddScoped<ICompositeSink<Apartment>, CompositeSink>(ctx =>
                    //{
                    //    var sinks = ctx.GetServices<ISink<Apartment>>();

                    //    var composite = new CompositeSink();

                    //    foreach (var service in sinks)
                    //    {
                    //        composite.Add((Sink)service);
                    //    }
                    //    return composite;
                    //});

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

            //sendGridSettings.Password = await store.GetOrLoadSettingAsync(sendGridSettings.Password);
            //sendGridSettings.UserName = await store.GetOrLoadSettingAsync(sendGridSettings.UserName);
        }

        private static void ConfigureSettings(HostBuilderContext hostContext, IServiceCollection services)
        {
            var storageSettings = new QueueStorageSettings();
            var appSettings = new AppSettings();
            //var sendGridSettings = new SendGridSettings();

            hostContext.Configuration.GetSection(nameof(QueueStorageSettings)).Bind(storageSettings);
            hostContext.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
            //hostContext.Configuration.GetSection(nameof(SendGridSettings)).Bind(sendGridSettings);

            LoadSecretSettings(storageSettings, appSettings)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(storageSettings);
            services.AddSingleton(appSettings);

            //services.AddSingleton(sendGridSettings);
        }
    }
}
