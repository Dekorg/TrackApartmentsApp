using System.Threading.Tasks;

namespace TrackApartmentsApp.Core.Contracts.Storage
{
    public interface IStorageWriteRepository<in T>
    {
        Task SaveAsync(string partitionKey, T item);

        Task DeleteAsync(string partitionKey, T item);
    }
}
