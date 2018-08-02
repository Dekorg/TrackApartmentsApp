using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackApartments.Contracts.Models;
using TrackApartments.User;
using TrackApartments.User.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsUserFunction
    {
        [FunctionName("TrackApartmentsUserFunction")]
        public static async Task Run([QueueTrigger("newqueueapartments", Connection = "QueueConnectionString")] string myQueueItem, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"{nameof(TrackApartmentsUserFunction)} has started.");

            try
            {
                var apartment = JsonConvert.DeserializeObject<Apartment>(myQueueItem);

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
