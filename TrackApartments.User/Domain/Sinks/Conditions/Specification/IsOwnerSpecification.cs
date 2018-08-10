using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Sinks.Conditions.Specification.Base.Abstract;

namespace TrackApartments.User.Domain.Sinks.Conditions.Specification
{
    public class IsOnlyCreatedByOwnerSpecification : CompositeSpecification<Order>
    {
        private readonly bool isOwner;

        public IsOnlyCreatedByOwnerSpecification(bool isOwner)
        {
            this.isOwner = isOwner;
        }


        public override bool IsSatisfiedBy(Order order)
        {
            return !isOwner || order.Apartment.IsCreatedByOwner == isOwner;
        }
    }
}
