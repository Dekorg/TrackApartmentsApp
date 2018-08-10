using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Sinks.Conditions.Interfaces;
using TrackApartments.User.Domain.Sinks.Conditions.Specification;

namespace TrackApartments.User.Domain.Sinks.Conditions
{
    public class EmailCondition : IEmailCondition
    {
        public bool IsValid(Order order)
        {
            var isNew = new IsNewSpecification(order.SinkSettings.EmailSettings.IsNewPeriod);
            var isOwner = new IsOnlyCreatedByOwnerSpecification(order.SinkSettings.EmailSettings.IsOnlyOwner);
            var priceLower = new PriceLowerSpecification(order.SinkSettings.EmailSettings.DesiredPriceBorder);

            return isNew
                .And(isOwner)
                .And(priceLower)
                .IsSatisfiedBy(order);
        }
    }
}
