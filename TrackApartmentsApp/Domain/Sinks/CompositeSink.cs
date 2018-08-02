using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartmentsApp.Domain.Sinks.Abstract;

namespace TrackApartmentsApp.Domain.Sinks
{
    public sealed class CompositeSink : Sink, ICompositeSink<Apartment>
    {
        private readonly List<Sink> children = new List<Sink>();

        public void Add(Sink component)
        {
            children.Add(component);
        }

        public void Remove(Sink component)
        {
            children.Remove(component);
        }

        public override async Task WriteAsync(Apartment message)
        {
            foreach (ISink<Apartment> sink in children)
            {
                await sink.WriteAsync(message);
            }
        }
    }
}
