using TrackApartments.Contracts.Models;

namespace TrackApartmentsApp.Domain.Sinks.Conditions.Interfaces
{
    public interface ICondition
    {
        bool IsValid(Apartment flat);
    }
}
