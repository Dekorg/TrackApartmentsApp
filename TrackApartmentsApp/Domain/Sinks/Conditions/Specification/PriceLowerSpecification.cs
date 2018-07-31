using System;
using System.Collections.Generic;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Sinks.Conditions.Abstract;

namespace TrackApartmentsApp.Domain.Sinks.Conditions
{
    public class PriceLowerSpecification : CompositeSpecification<Apartment>
    {
        private readonly int price;

        public PriceLowerSpecification(int price)
        {
            this.price = price;
        }

        public override bool IsSatisfiedBy(Apartment apartment)
        {
            return apartment.Price <= price;
        }
    }
}

//public IEnumerable<Apartment> GetValid(List<Apartment> list)
//{
//foreach (var item in list)
//{
//var deltaTime = (int)DateTime.Now.Subtract(item.Created).TotalDays;

//    if (item.IsCreatedByOwner && deltaTime <= 1 && item.Price <= 350)
//{
//    yield return item;
//}
//}
//}
