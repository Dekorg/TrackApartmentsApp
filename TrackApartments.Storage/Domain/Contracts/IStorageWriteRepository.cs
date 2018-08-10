using System.Threading.Tasks;

namespace TrackApartments.Storage.Domain.Contracts
{
    public interface IStorageWriteRepository<in T>
    {
        Task SaveAsync(string partitionKey, T item);

        Task DeleteAsync(string partitionKey, T item);
    }
}
