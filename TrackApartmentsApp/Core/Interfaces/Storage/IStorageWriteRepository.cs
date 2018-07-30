using System.Threading.Tasks;

namespace TrackApartmentsApp.Core.Interfaces.Storage
{
    public interface IStorageWriteRepository<in T>
    {
        Task SaveAsync(string partitionKey, T item);

        Task DeleteAsync(string partitionKey, T item);
    }
}
