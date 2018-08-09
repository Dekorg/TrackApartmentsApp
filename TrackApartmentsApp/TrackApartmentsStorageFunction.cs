using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Storage;
using TrackApartments.Storage.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsStorageFunction
    {
        [FunctionName("TrackApartmentsStorageFunction")]
        public static async Task Run(
            [QueueTrigger("queueapartments", Connection = "QueueConnectionString")]Apartment flat,
            ILogger log,
            ExecutionContext context,
            [Queue("newqueueapartments", Connection = "QueueConnectionString")] ICollector<Apartment> queueItems)
        {
            log.LogDebug($"{nameof(TrackApartmentsStorageFunction)} has started.", flat);

            try
            {
                var di = new StorageHostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (StorageApartmentService)host.Services.GetService(typeof(StorageApartmentService));

                if (await app.IsNewItemAsync(flat))
                {
                    queueItems.Add(flat);
                }
            }
            catch (Exception ex)
            {
                var message = $"{nameof(TrackApartmentsStorageFunction)} function failed.";
                log.LogError(message, ex);
                throw;
            }
        }
    }
}
