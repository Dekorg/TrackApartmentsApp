using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace TrackApartmentsApp
{
    public static class TrackApartmentFunction
    {
        public static readonly HttpClient CachedClient = new HttpClient();

        [FunctionName("TrackApartmentFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogDebug($"{nameof(TrackApartmentFunction)} has started.", myTimer);

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var onlinerUrl = config.GetSection("SourceSettings:OnlinerURL").Value;
            var kufarUrl = config.GetSection("SourceSettings:KufarURL").Value;

            if (!string.IsNullOrEmpty(onlinerUrl))
            {
                var urls = onlinerUrl.Split(',');
                Array.ForEach(urls, x => CachedClient.GetAsync(x));
            }

            //CachedClient.GetAsync(kufarUrl);
        }
    }
}

