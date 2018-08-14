using System.Threading.Tasks;

namespace TrackApartments.Contracts
{
    public interface IQueueWriter<in T>
    {
        Task WriteAsync(T apartment);
    }
}
