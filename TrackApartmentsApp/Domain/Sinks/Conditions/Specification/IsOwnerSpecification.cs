using TrackApartments.Contracts.Models;
using TrackApartmentsApp.Domain.Sinks.Conditions.Specification.Base.Abstract;

namespace TrackApartmentsApp.Domain.Sinks.Conditions.Specification
{
    public class IsOwnerSpecification : CompositeSpecification<Apartment>
    {
        public override bool IsSatisfiedBy(Apartment apartment)
        {
            return apartment.IsCreatedByOwner;
        }
    }
}
