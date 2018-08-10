using System;
using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Sinks.Conditions.Specification.Base.Abstract;

namespace TrackApartments.User.Domain.Sinks.Conditions.Specification
{
    public class IsNewSpecification : CompositeSpecification<Order>
    {
        private readonly int daysItConsideredNew;

        public IsNewSpecification(int daysItConsideredNew)
        {
            this.daysItConsideredNew = daysItConsideredNew;
        }

        public override bool IsSatisfiedBy(Order order)
        {
            var deltaTime = (int)DateTime.Now.Subtract(order.Apartment.Created).TotalDays;
            return deltaTime <= daysItConsideredNew;
        }
    }
}
