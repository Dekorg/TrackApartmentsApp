using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Storage.DeleteDups;
using TrackApartments.Storage.DeleteDups.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsDeleteDupsStorageFunction
    {
        [FunctionName("TrackApartmentsDeleteDupsStorageFunction")]
        public static async Task Run([TimerTrigger("0 0 1 * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogDebug($"{nameof(TrackApartmentsDeleteDupsStorageFunction)} has started.");

            try
            {
                var di = new StorageDeleteDupsHostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (StorageDeleteDupsService)host.Services.GetService(typeof(StorageDeleteDupsService));

                await app.DeleteDupsAsync();
            }
            catch (Exception ex)
            {
                var message = $"{nameof(TrackApartmentsStorageFunction)} function failed.";
                log.LogError(message, ex);
            }
        }
    }
}
