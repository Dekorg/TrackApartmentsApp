using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrackApartments.Data.Contracts
{
    public interface IStorageReadRepository<T>
    {
        Task<List<T>> LoadAsync(string partitionKey);
    }
}
