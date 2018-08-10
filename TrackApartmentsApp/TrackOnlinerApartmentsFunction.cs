using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Onliner;
using TrackApartments.Onliner.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackOnlinerApartmentsFunction
    {
        [FunctionName("TrackOnlinerApartmentsFunction")]
        public static async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req,
            ILogger log,
            ExecutionContext context,
            [Queue("queueapartments", Connection = "QueueConnectionString")] ICollector<Apartment> queueItems)
        {
            log.LogDebug($"{nameof(TrackOnlinerApartmentsFunction)} has started.");

            string url = req.Query["url"];

            if (String.IsNullOrEmpty(url))
            {
                return new BadRequestObjectResult("Please, pass an url on the query string.");
            }

            try
            {
                var di = new OnlinerHostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (OnlinerApartmentService)host.Services.GetService(typeof(OnlinerApartmentService));

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
