using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Sinks.Abstract;
using TrackApartmentsApp.Domain.Sinks.Conditions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TrackApartmentsApp.Domain.Sinks
{
    public class SmsSink : Sink
    {
        private readonly TwilioSettings twilioSettings;
        private readonly ISmsCondition condition;
        private readonly ILogger logger;

        public SmsSink(TwilioSettings twilioSettings, ISmsCondition condition, ILogger logger)
        {
            this.twilioSettings = twilioSettings;
            this.condition = condition;
            this.logger = logger;
            TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
        }

        public override async Task WriteAsync(Apartment item)
        {
            if (condition.IsValid(item))
            {
                var to = new PhoneNumber(twilioSettings.PhoneNumberTo);

                var message = await MessageResource.CreateAsync(
                    to,
                    from: new PhoneNumber(twilioSettings.PhoneNumberFrom),
                    body: item.ToString());

                logger.LogInformation("Sms message has been sent.", message, item);
            }
            else
            {
                logger.LogInformation("Sms message has been declined.", item);
            }
        }
    }
}
