using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsTwilioFunction
    {
        [FunctionName("TrackApartmentsTwilioFunction")]
        [return: TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "+18509002504")]
        public static CreateMessageOptions Run([QueueTrigger("neworderssmsqueue", Connection = "QueueConnectionString")] Order order, ILogger log)
        {
            log.LogInformation($"{nameof(TrackApartmentsTwilioFunction)} has started.", order);

            if (order.Apartment == null || String.IsNullOrEmpty(order.User.Phone))
            {
                throw new ArgumentException(nameof(order));
            }

            var msgOptions = new CreateMessageOptions(new PhoneNumber(order.User.Phone))
            {
                Body = order.Apartment.ToString()
            };

            log.LogInformation("New sms has been sent.", msgOptions, order);

            return null;
        }
    }
}

