using System;
using System.Collections.Generic;
using System.Text;

namespace TrackApartments.Contracts.Models
{
    public class UserSettings
    {
        public SinkSettings EmailSettings { get; set; }

        public SinkSettings SmsSettings { get; set; }
    }
}
