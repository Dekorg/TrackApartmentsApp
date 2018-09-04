using System;
using System.Collections.Generic;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Storage.DeleteDups.Domain.Comparers
{
    public class PriceAndAddressEqualityComparer : IEqualityComparer<Apartment>
    {
        public const int PriceEpsilon = 10;

        public bool Equals(Apartment item1, Apartment item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            return item1.Address == item2.Address &&
                   (item1.Price.Equals(item2.Price) || Math.Abs(item1.Price - item2.Price) < PriceEpsilon)
                   && item1.Rooms == item2.Rooms;
        }

        public int GetHashCode(Apartment item)
        {
            int hash = 13;

            var roundedPrice = item.Price - item.Price % 10;

            hash = hash * 21 + item.Uri.GetHashCode();
            hash = hash * 21 + roundedPrice.GetHashCode();
            hash = hash * 21 + item.Rooms.GetHashCode();

            return hash;
        }
    }
}


