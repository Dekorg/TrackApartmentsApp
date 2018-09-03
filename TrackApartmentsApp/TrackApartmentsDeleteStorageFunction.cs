using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Storage.Delete;
using TrackApartments.Storage.Delete.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsDeleteStorageFunction
    {
        [FunctionName("TrackApartmentsDeleteStorageFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log,
            ExecutionContext context)
        {
            log.LogDebug($"{nameof(TrackApartmentsDeleteStorageFunction)} has started.");

            try
            {
                var di = new StorageDeleteHostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (StorageDeleteService)host.Services.GetService(typeof(StorageDeleteService));

                await app.DeleteObsoleteAsync();
            }
            catch (Exception ex)
            {
                var message = $"{nameof(TrackApartmentsStorageFunction)} function failed.";
                log.LogError(message, ex);

            }
        }
    }
}
