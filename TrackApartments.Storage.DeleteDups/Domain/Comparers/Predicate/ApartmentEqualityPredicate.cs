using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Storage.DeleteDups.Domain.Comparers.Predicate
{
    public class ApartmentEqualityPredicate
    {
        public const int PriceEpsilon = 10;

        public bool CompositePredicate(Apartment item1, Apartment item2)
        {
            return AddressAndPhonePredicate(item1, item2) ||
                   UriAndPricePredicate(item1, item2) ||
                   PriceAndAddressPredicate(item1, item2);
        }


        public bool AddressAndPhonePredicate(Apartment item1, Apartment item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            return item1.Address == item2.Address &&
                   item1.Phones.Intersect(item2.Phones).Any();
        }

        public bool UriAndPricePredicate(Apartment item1, Apartment item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            return item1.Uri == item2.Uri &&
                   (item1.Price.Equals(item2.Price) || Math.Abs(item1.Price - item2.Price) < PriceEpsilon);
        }

        public bool PriceAndAddressPredicate(Apartment item1, Apartment item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            return item1.Address == item2.Address &&
                   (item1.Price.Equals(item2.Price) || Math.Abs(item1.Price - item2.Price) < PriceEpsilon)
                   && item1.Rooms == item2.Rooms;
        }
    }
}
