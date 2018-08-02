using TrackApartments.Contracts.Models;
using TrackApartmentsApp.Domain.Sinks.Conditions.Interfaces;
using TrackApartmentsApp.Domain.Sinks.Conditions.Specification;

namespace TrackApartmentsApp.Domain.Sinks.Conditions
{
    public class SmsCondition : ISmsCondition
    {
        public bool IsValid(Apartment flat)
        {
            var isNew = new IsNewSpecification(1);
            var isOwner = new IsOwnerSpecification();
            var priceLower = new PriceLowerSpecification(350);

            return isNew
                .And(isOwner)
                .And(priceLower)
                    .IsSatisfiedBy(flat);
        }
    }
}
