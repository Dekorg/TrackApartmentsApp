using System;

namespace TrackApartments.Contracts.Models
{
    public class SinkSettings
    {
        public bool IsOnlyOwner { get; set; }

        public int DesiredPriceBorder { get; set; }

        public int IsNewPeriod { get; set; }
    }
}
