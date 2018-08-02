using System;
using TrackApartments.Contracts.Models;
using TrackApartmentsApp.Domain.Sinks.Conditions.Specification.Base.Abstract;

namespace TrackApartmentsApp.Domain.Sinks.Conditions.Specification
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
