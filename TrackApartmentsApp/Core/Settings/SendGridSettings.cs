using System;
using System.Collections.Generic;
using System.Text;

namespace TrackApartmentsApp.Core.Settings
{
    public class SendGridSettings
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public int Port { get; set; }
    }
}
