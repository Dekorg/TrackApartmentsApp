using System;

namespace TrackApartments.Contracts.Models
{
    public class UserSinkSettings
    {
        public SinkSettings EmailSettings { get; set; }

        public SinkSettings SmsSettings { get; set; }
    }
}
