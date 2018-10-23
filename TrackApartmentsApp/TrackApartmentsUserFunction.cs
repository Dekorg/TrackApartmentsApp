using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.User;
using TrackApartments.User.Infrastructure.Configuration;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsUserFunction
    {
        [FunctionName("TrackApartmentsUserFunction")]
        public static async Task Run(
            [QueueTrigger("newqueueapartments", Connection = "QueueConnectionString")]
            Apartment apartment,
            ILogger log,
            ExecutionContext context)
        {
            log.LogDebug($"{nameof(TrackApartmentsUserFunction)} has started.");

            try
            {
                var di = new UserHostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (UserOrderService)host.Services.GetService(typeof(UserOrderService));
                await app.SaveAsync(apartment);
            }
            catch (Exception ex)
            {
                var message = $"{nameof(TrackOnlinerApartmentsFunction)} function failed";
                log.LogError(message, ex);
                throw;
            }
        }
    }
}


