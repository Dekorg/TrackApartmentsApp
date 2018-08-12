using System.Threading.Tasks;
using TrackApartments.Contracts.Models;

namespace TrackApartments.User.Domain.Sinks.Abstract
{
    public abstract class Sink : ISink<Order>
    {
        public abstract Task WriteAsync(Order message);
    }
}
