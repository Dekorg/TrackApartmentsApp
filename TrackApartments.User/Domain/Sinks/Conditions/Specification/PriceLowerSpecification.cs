using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Sinks.Conditions.Specification.Base.Abstract;

namespace TrackApartments.User.Domain.Sinks.Conditions.Specification
{
    public class PriceLowerSpecification : CompositeSpecification<Order>
    {
        private readonly int price;

        public PriceLowerSpecification(int price)
        {
            this.price = price;
        }

        public override bool IsSatisfiedBy(Order order)
        {
            return order.Apartment.Price <= price;
        }
    }
}
