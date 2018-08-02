using TrackApartments.Contracts.Models;
using TrackApartmentsApp.Domain.Sinks.Conditions.Specification.Base.Abstract;

namespace TrackApartmentsApp.Domain.Sinks.Conditions.Specification
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
