using System.Threading.Tasks;

namespace TrackApartmentsApp.Domain.Sinks.Abstract
{
    public interface ISink<T>
    {
        Task WriteAsync(T message);
    }
}
