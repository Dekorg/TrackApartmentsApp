using System;
using System.Collections.Generic;
using System.Text;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Domain.Sinks.Conditions
{
    public class EmailCondition : IEmailCondition
    {
        public bool IsValid(Apartment flat)
        {
            throw new NotImplementedException();
        }
    }
}
