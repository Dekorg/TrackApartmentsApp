using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Onliner;
using TrackApartments.Onliner.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackOnlinerApartmentsFunction
    {
        [FunctionName("TrackOnlinerApartmentsFunction")]
        public static async Task<ActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogDebug($"{nameof(TrackOnlinerApartmentsFunction)} has started.");

            string url = req.Query["url"];

            if (string.IsNullOrEmpty(url))
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
