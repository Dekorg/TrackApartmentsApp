using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackApartments.Contracts.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsTwilioFunction
    {
        [FunctionName("TrackApartmentsTwilioFunction")]
        [return: TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "+18509002504")]
        public static CreateMessageOptions Run([QueueTrigger("newqueueapartments", Connection = "QueueConnectionString")] string myQueueItem, ILogger log)
        {
            var apartment = JsonConvert.DeserializeObject<Apartment>(myQueueItem);

            var msgOptions = new CreateMessageOptions(new PhoneNumber("+375291602219"))
            {
                Body = apartment.ToString()
            };

            return msgOptions;
        }
    }
}

