using System;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Sinks.Conditions.Abstract;

namespace TrackApartmentsApp.Domain.Sinks.Conditions
{
    public class IsNewSpecification : CompositeSpecification<Apartment>
    {
        private readonly int daysItConsideredNew;

        public IsNewSpecification(int daysItConsideredNew)
        {
            this.daysItConsideredNew = daysItConsideredNew;
        }

        public override bool IsSatisfiedBy(Apartment apartment)
        {
            var deltaTime = (int)DateTime.Now.Subtract(apartment.Created).TotalDays;
            return deltaTime <= daysItConsideredNew;
        }
    }
}
