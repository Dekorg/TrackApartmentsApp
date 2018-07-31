using System;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Domain.Sinks.Conditions
{
    public interface ICondition
    {
        bool IsValid(Apartment flat);
    }
}
