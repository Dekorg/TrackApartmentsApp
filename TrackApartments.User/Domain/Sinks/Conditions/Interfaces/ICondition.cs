using TrackApartments.Contracts.Models;

namespace TrackApartments.User.Domain.Sinks.Conditions.Interfaces
{
    public interface ICondition
    {
        bool IsValid(Order order);
    }
}
