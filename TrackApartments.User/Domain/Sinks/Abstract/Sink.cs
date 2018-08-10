using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartmentsApp.Domain.Sinks.Abstract;

namespace TrackApartments.User.Domain.Sinks.Abstract
{
    public abstract class Sink : ISink<Order>
    {
        public abstract Task WriteAsync(Order message);
    }
}
