using System;

namespace TrackApartments.Contracts.Models
{
    public class Order
    {
        public Apartment Apartment { get; set; }

        public User User { get; set; }
    }
}
