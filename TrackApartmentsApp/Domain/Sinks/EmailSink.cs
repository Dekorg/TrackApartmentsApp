using System;
using System.Threading.Tasks;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Sinks.Abstract;

namespace TrackApartmentsApp.Domain.Sinks
{
    public class EmailSink : Sink
    {
        public override async Task WriteAsync(Apartment message)
        {
            Console.WriteLine(message);
        }
    }
}
