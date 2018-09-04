using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Storage.DeleteDups.Domain.Comparers
{
    public class AddressAndPhoneEqualityComparer : IEqualityComparer<Apartment>
    {
        public bool Equals(Apartment item1, Apartment item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            return item1.Address == item2.Address &&
                   item1.Phones.Intersect(item2.Phones).Any();
        }

        public int GetHashCode(Apartment item)
        {
            int hash = 13;

            hash = hash * 21 + item.Address.GetHashCode();

            foreach (var phone in item.Phones)
            {
                hash = hash * 21 + phone.GetHashCode();
            }

            return hash;
        }
    }
}




//Address == item.Address && ;