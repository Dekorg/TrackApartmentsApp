using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartments.Get;
using TrackApartments.Get.Infrastructure.Configuration;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsGetFunction
    {
        [FunctionName("TrackApartmentsGetFunction")]
        //[ClaimRequirement(MyClaimTypes.Permission, "CanReadResource")]
        public static async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogDebug($"{nameof(TrackApartmentsGetFunction)} has started.");

            string id = req.Query["id"];

            if (!String.IsNullOrEmpty(id) && !Guid.TryParse(id, out Guid guid))
            {
                log.LogDebug($"Failed to parse guid: {id}");
                return new BadRequestObjectResult("Guid of the apartment is invalid.");
            }

            try
            {
                var di = new GetApartmentHostConfigurator();
                var host = di.BuildHost(context, log);
                host.Start();

                var app = (GetApartmentService)host.Services.GetService(typeof(GetApartmentService));

                if (String.IsNullOrEmpty(id))
                {
                    return new OkObjectResult(await app.GetItemsAsync());
                }
                return new OkObjectResult(await app.GetItemAsync(guid));
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
