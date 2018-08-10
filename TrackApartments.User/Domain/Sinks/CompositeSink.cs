using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Sinks.Abstract;
using TrackApartmentsApp.Domain.Sinks.Abstract;

namespace TrackApartments.User.Domain.Sinks
{
    public sealed class CompositeSink : Sink, ICompositeSink<Order>
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

        public override async Task WriteAsync(Order message)
        {
            foreach (ISink<Order> sink in children)
            {
                await sink.WriteAsync(message);
            }
        }
    }
}
