using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackApartments.Contracts.Models;
using TrackApartments.Storage;
using TrackApartments.Storage.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsStorageFunction
    {
        [FunctionName("TrackApartmentsStorageFunction")]
        public static async Task Run([QueueTrigger("queueapartments", Connection = "QueueConnectionString")]string myQueueItem, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"{nameof(TrackApartmentsStorageFunction)} has started.", myQueueItem);

            try
            {
                var di = new StorageHostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (StorageApartmentService)host.Services.GetService(typeof(StorageApartmentService));
                var apartment = JsonConvert.DeserializeObject<Apartment>(myQueueItem);

                await app.SaveIfNewItemAsync(apartment);
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
