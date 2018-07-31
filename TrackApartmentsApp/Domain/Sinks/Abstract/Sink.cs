using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Domain.Sinks.Abstract
{
    public abstract class Sink : ISink<Apartment>
    {
        public abstract Task WriteAsync(Apartment message);
    }
}
