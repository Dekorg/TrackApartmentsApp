using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TrackApartments.Data.Contracts
{
    public interface IStorageWorker
    {
        Task SaveAsync<T>(T item) where T : ITableEntity, new();

        Task DeleteAsync<T>(T item) where T : ITableEntity, new();

        Task<List<T>> LoadListAsync<T>(string key) where T : ITableEntity, new();

        Task<T> LoadAsync<T>(string partitionKey, string rowKey) where T : ITableEntity, new();
    }
}
