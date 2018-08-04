using System;

namespace TrackApartments.Contracts.Models
{
    public class Order
    {
        public Apartment Apartment { get; set; }

        public UserInfo UserInfo { get; set; }

        public UserSinkSettings SinkSettings { get; set; }
    }
}
