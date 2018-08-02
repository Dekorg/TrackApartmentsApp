using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Onliner.Settings;
using TrackApartmentsApp.Domain.Sinks.Abstract;
using TrackApartmentsApp.Domain.Sinks.Conditions.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TrackApartmentsApp.Domain.Sinks
{
    public sealed class SmsSink : Sink
    {
        private readonly ISmsCondition condition;
        private readonly ILogger logger;

        public SmsSink(ISmsCondition condition, ILogger logger)
        {
            this.condition = condition;
            this.logger = logger;
            //TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
        }

        public override async Task WriteAsync(Apartment item)
        {
            //            if (condition.IsValid(item))
            //            {
            //                var to = new PhoneNumber(twilioSettings.PhoneNumberTo);
            //
            //                var message = await MessageResource.CreateAsync(
            //                    to,
            //                    from: new PhoneNumber(twilioSettings.PhoneNumberFrom),
            //                    body: item.ToString());
            //
            //                logger.LogInformation("Sms message has been sent.", message, item);
            //            }
            //            else
            //            {
            //                logger.LogInformation("Sms message has been declined.", item);
            //            }
        }
    }
}
