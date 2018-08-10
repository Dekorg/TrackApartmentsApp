using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Sinks.Conditions.Interfaces;
using TrackApartments.User.Domain.Sinks.Conditions.Specification;

namespace TrackApartments.User.Domain.Sinks.Conditions
{
    public class SmsCondition : ISmsCondition
    {
        public bool IsValid(Order order)
        {
            var isNew = new IsNewSpecification(order.SinkSettings.SmsSettings.IsNewPeriod);
            var isOwner = new IsOnlyCreatedByOwnerSpecification(order.SinkSettings.SmsSettings.IsOnlyOwner);
            var priceLower = new PriceLowerSpecification(order.SinkSettings.SmsSettings.DesiredPriceBorder);

            return isNew
                .And(isOwner)
                .And(priceLower)
                    .IsSatisfiedBy(order);
        }
    }
}
