using System;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;

namespace TrackApartmentsApp.Domain.Sinks.Abstract
{
    public abstract class Sink : ISink<Apartment>
    {
        public abstract Task WriteAsync(Apartment message);
    }
}
