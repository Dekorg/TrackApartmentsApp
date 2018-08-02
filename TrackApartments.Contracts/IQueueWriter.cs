using System.Threading.Tasks;

namespace TrackApartments.Contracts
{
    public interface IQueueWriter<T>
    {
        Task WriteAsync(T apartment);
    }
}
