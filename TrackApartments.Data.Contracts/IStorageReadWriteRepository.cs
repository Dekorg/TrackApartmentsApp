using System;
using System.Threading.Tasks;

namespace TrackApartments.Data.Contracts
{
    public interface IStorageReadWriteRepository<in T>
    {
        Task DeleteAsync(string partitionKey, string rowKey);
    }
}
