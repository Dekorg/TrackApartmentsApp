using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Domain.Conditions;
using TrackApartmentsApp.Domain.Connectors.Abstract;
using TrackApartmentsApp.Domain.Connectors.OnlinerConnector;
using TrackApartmentsApp.Domain.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TrackApartmentsApp
{
    public class ApartmentService
    {
        private readonly IStorageConnector storageConnector;
        private readonly IOnlinerConnector onlinerConnector;
        private readonly TwilioSettings twilioSettings;

        public ApartmentService(IStorageConnector storageConnector, IOnlinerConnector onlinerConnector, TwilioSettings twilioSettings)
        {
            this.storageConnector = storageConnector;
            this.onlinerConnector = onlinerConnector;
            this.twilioSettings = twilioSettings;
        }

        public async Task StartAsync(ILogger log, ExecutionContext context)
        {
            var results = await GetOnlinerApartments(log, onlinerConnector);
            var items = await GetNewlyCreatedItemsAsync(log, storageConnector, results);

            SendSmsMessages(items);
        }

        private static async Task<List<Apartment>> GetOnlinerApartments(ILogger log, IOnlinerConnector connector)
        {
            var results = await connector.GetAsync();

            foreach (var item in results)
            {
                log.LogDebug($"All items: {item.Address}, url: {item.Uri}");
            }

            return results;
        }

        private async Task<List<Apartment>> GetNewlyCreatedItemsAsync(ILogger log, IStorageConnector storageConnector, List<Apartment> results)
        {
            var newItems = await storageConnector.MergeAndGetNewAsync(results);

            log.LogDebug($"Count new items: {newItems.Count}");

            SmsCondition condition = new SmsCondition();
            var validResults = condition.GetValid(newItems).ToList();

            foreach (var item in validResults)
            {
                log.LogDebug($"New items: {item.Address} url: {item.Uri}");
            }
            return validResults;
        }

        private void SendSmsMessages(List<Apartment> validResults)
        {
            TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);

            foreach (var item in validResults)
            {
                var to = new PhoneNumber(twilioSettings.PhoneNumberTo);
                var message = MessageResource.Create(
                    to,
                    from: new PhoneNumber(twilioSettings.PhoneNumberFrom),
                    body: item.ToString());

                Console.WriteLine(message.Sid);
            }
        }
    }
}
