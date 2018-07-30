using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrackApartmentsApp.Core.Interfaces.Storage
{
    public interface IStorageReadRepository<T>
    {
        Task<List<T>> LoadAsync(string partitionKey);
    }
}
