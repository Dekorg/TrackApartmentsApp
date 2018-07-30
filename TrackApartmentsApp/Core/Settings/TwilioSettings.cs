using System;
using System.Collections.Generic;
using System.Text;

namespace TrackApartmentsApp.Core.Settings
{
    public class TwilioSettings
    {
        public string AccountSid { get; set; }

        public string AuthToken { get; set; }

        public string PhoneNumberTo { get; set; }

        public string PhoneNumberFrom { get; set; }
    }
}
