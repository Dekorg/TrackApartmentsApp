using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartmentsApp.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackApartmentFunction
    {
        [FunctionName("TrackApartmentFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            try
            {
                var di = new HostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (ApartmentService)host.Services.GetService(typeof(ApartmentService));

                await app.StartAsync(context);
            }
            catch (Exception ex)
            {
                var message = "Function failed";
                log.LogError(message, ex);
                throw;
            }
        }
    }
}

