using System;
using System.Collections.Generic;
using System.Text;

namespace TrackApartments.Contracts.Models
{
    public class Order
    {
        public Apartment Apartment { get; set; }

        public User User { get; set; }
    }
}
