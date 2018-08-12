using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Core.Secrets;
using TrackApartments.User.Domain.Queue;
using TrackApartments.User.Domain.Queue.Contracts;
using TrackApartments.User.Domain.Sinks;
using TrackApartments.User.Domain.Sinks.Abstract;
using TrackApartments.User.Domain.Sinks.Conditions;
using TrackApartments.User.Domain.Sinks.Conditions.Interfaces;
using TrackApartments.User.Settings;

namespace TrackApartments.User.Infrastructure.Configuration
{
    public class UserHostConfigurator
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

                    services.AddScoped<ISmsQueueWriter<Order>, SmsQueueWriter>();
                    services.AddScoped<IEmailQueueWriter<Order>, EmailQueueWriter>();

                    services.AddScoped<IEmailCondition, EmailCondition>();
                    services.AddScoped<ISmsCondition, SmsCondition>();

                    services.AddScoped<ISink<Order>, SmsSink>();
                    services.AddScoped<ISink<Order>, EmailSink>();
                    services.AddScoped<ICompositeSink<Order>, CompositeSink>(ctx =>
                    {
                        var sinks = ctx.GetServices<ISink<Order>>();

                        var composite = new CompositeSink();

                        foreach (var service in sinks)
                        {
                            composite.Add((Sink)service);
                        }
                        return composite;
                    });

                    services.AddScoped<UserOrderService>();

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
            var queueStorageSettings = new QueueStorageSettings();
            var appSettings = new AppSettings();

            hostContext.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
            hostContext.Configuration.GetSection(nameof(QueueStorageSettings)).Bind(queueStorageSettings);

            LoadSecretSettings(queueStorageSettings, appSettings)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(queueStorageSettings);
            services.AddSingleton(appSettings);
        }
    }
}
