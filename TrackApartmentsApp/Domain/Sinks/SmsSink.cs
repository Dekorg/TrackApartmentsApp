using System;
using System.Threading.Tasks;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Sinks.Abstract;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TrackApartmentsApp.Domain.Sinks
{
    public class SmsSink : Sink
    {
        private readonly TwilioSettings twilioSettings;

        public SmsSink(TwilioSettings twilioSettings)
        {
            this.twilioSettings = twilioSettings;
            TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
        }

        public override async Task WriteAsync(Apartment item)
        {
            var to = new PhoneNumber(twilioSettings.PhoneNumberTo);

            await MessageResource.CreateAsync(
                to,
                from: new PhoneNumber(twilioSettings.PhoneNumberFrom),
                body: item.ToString());
        }
    }
}
