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
        [return: TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "+14693738930")]
        public static CreateMessageOptions Run([QueueTrigger("neworderssmsqueue", Connection = "QueueConnectionString")] Order order, ILogger log)
        {
            log.LogDebug($"{nameof(TrackApartmentsTwilioFunction)} has started.", order);

            if (order.Apartment == null || String.IsNullOrEmpty(order.UserInfo.Phone))
            {
                throw new ArgumentException(nameof(order));
            }

            var msgOptions = new CreateMessageOptions(new PhoneNumber(order.UserInfo.Phone))
            {
                Body = order.Apartment.ToString()
            };

            log.LogDebug("New sms has been sent.", msgOptions, order);

            return msgOptions;
        }
    }
}

