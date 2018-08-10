using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrackApartments.Storage.Domain.Contracts
{
    public interface IStorageReadRepository<T>
    {
        Task<List<T>> LoadAsync(string partitionKey);
    }
}
