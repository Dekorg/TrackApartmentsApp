using System;
using System.Collections.Generic;
using System.Text;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Sinks.Conditions.Abstract;

namespace TrackApartmentsApp.Domain.Sinks.Conditions
{
    public class IsOwnerSpecification : CompositeSpecification<Apartment>
    {
        public override bool IsSatisfiedBy(Apartment apartment)
        {
            return apartment.IsCreatedByOwner;
        }
    }
}
