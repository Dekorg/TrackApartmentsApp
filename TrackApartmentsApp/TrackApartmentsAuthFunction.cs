using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TrackApartments.Auth;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsAuthFunction
    {
        [FunctionName("TrackApartmentsAuthFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var authentication = req.TryAuthenticate(out AuthenticationModel model);

            //string name = req.Query["name"];

            //string requestBody = new StreamReader(req.Body).ReadToEnd();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            return authentication
                ? (ActionResult)new OkObjectResult($"This is my Auth: {model}, {model.AccessToken}")
                : new BadRequestObjectResult("Not authorized.");
        }
    }
}
