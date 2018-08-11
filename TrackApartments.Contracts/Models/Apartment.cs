using System;
using System.Collections.Generic;
using System.Linq;
using TrackApartments.Contracts.Enums;

namespace TrackApartments.Contracts.Models
{
    public class Apartment
    {
        public string Address { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public List<string> Phones { get; set; } = new List<string>();

        public bool IsCreatedByOwner { get; set; }

        public float Price { get; set; }

        public int Rooms { get; set; }

        public Uri Uri { get; set; }

        public DataSource Source { get; set; }

        public override string ToString()
        {
            return $"{Address}, {Phones.FirstOrDefault()}, ${Price}, R:{Rooms}, {Uri}";
        }

        public override bool Equals(Object other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            var item = (Apartment)other;
            return Address == item.Address &&
                   Uri == item.Uri;
        }

        public override int GetHashCode()
        {
            return this.Address.GetHashCode() ^ Uri.GetHashCode();
        }
    }
}
