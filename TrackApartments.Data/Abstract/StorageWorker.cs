using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TrackApartments.Data.Contracts;
using TrackApartments.Data.Contracts.Settings;

namespace TrackApartments.Data.Abstract
{
    public class StorageWorker : IStorageWorker
    {
        private CloudTable table;

        public StorageWorker(StorageSettings settings)
        {
            var storageAccount = CloudStorageAccount.Parse(settings.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(settings.TableName);

            //todo rewrite to CosmosDb. Currently in preview.
            table.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

        public async Task SaveAsync<T>(T item) where T : ITableEntity, new()
        {
            var operation = TableOperation.InsertOrReplace(item);
            await table.ExecuteAsync(operation);
        }

        public async Task<List<T>> LoadListAsync<T>(string key) where T : ITableEntity, new()
        {
            TableContinuationToken token = null;
            var entities = new List<T>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token);
                entities.AddRange(queryResult);
                token = queryResult.ContinuationToken;
            }
            while (token != null);

            return entities.Where(x => x.PartitionKey == key).ToList();
        }

        public async Task DeleteAsync<T>(T item) where T : ITableEntity, new()
        {
            item.ETag = "*";
            var operation = TableOperation.Delete(item);
            await table.ExecuteAsync(operation);
        }
    }
}
