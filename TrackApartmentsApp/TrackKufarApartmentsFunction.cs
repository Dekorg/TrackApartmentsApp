using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Kufar;
using TrackApartments.Kufar.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackKufarApartmentsFunction
    {
        [FunctionName("TrackKufarApartmentsFunction")]
        public static async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req,
            ILogger log,
            ExecutionContext context,
            [Queue("queueapartments", Connection = "QueueConnectionString")] ICollector<Apartment> queueItems)
        {
            log.LogDebug($"{nameof(TrackKufarApartmentsFunction)} has started.");

            string url = req.Query["url"];

            if (String.IsNullOrEmpty(url))
            {
                return new BadRequestObjectResult("Please, pass an url on the query string.");
            }

            try
            {
                var di = new KufarHostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (KufarApartmentService)host.Services.GetService(typeof(KufarApartmentService));

                var result = await app.GetAppartments(url);
                result.ForEach(queueItems.Add);

                return new OkObjectResult(result);
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
