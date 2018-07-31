using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TrackApartmentsApp.Core.Contracts.Storage
{
    public interface IStorageWorker
    {
        Task SaveAsync<T>(T item) where T : ITableEntity, new();

        Task DeleteAsync<T>(T item) where T : ITableEntity, new();

        Task<List<T>> LoadListAsync<T>(string key) where T : ITableEntity, new();
    }
}
