using System.Threading.Tasks;

namespace TrackApartments.User.Domain.Sinks.Abstract
{
    public interface ISink<T>
    {
        Task WriteAsync(T message);
    }
}
