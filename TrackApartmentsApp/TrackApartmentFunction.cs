using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Data;
using TrackApartmentsApp.Data.PageParsers.Onliner;
using TrackApartmentsApp.Data.Storage;
using TrackApartmentsApp.Data.Storage.Abstract;
using TrackApartmentsApp.Domain.Conditions;
using TrackApartmentsApp.Domain.Connectors.OnlinerConnector;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Secrets;
using TrackApartmentsApp.Infrastructure.Configuration;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace TrackApartmentsApp
{
    public static class TrackApartmentFunction
    {
        [FunctionName("TrackApartmentFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log,
            ExecutionContext context)
        {
            try
            {
                //await StartAsync(log, context);

                var di = new DependencyConfigurator();
                var host = di.BuildHost(context);

                host.Start();
                var app = (ApartmentService)host.Services.GetService(typeof(ApartmentService));

                await app.StartAsync(log, context);
            }
            catch (Exception ex)
            {
                var message = $"Function failed.";
                log.LogError(message, ex);
                throw;
            }
        }
    }
}

