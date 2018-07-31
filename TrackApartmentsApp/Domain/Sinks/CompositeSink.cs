using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Domain.Sinks.Abstract
{
    public class CompositeSink : Sink, ICompositeSink<Apartment>
    {
        private List<Sink> children = new List<Sink>();

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
