using System;
using System.Threading.Tasks;
using TrackApartments.Storage.Delete.Domain.Contracts;

namespace TrackApartments.Storage.Delete
{
    public class StorageDeleteService
    {
        private readonly IStorageConnector storageConnector;

        public StorageDeleteService(IStorageConnector storageConnector)
        {
            this.storageConnector = storageConnector;
        }

        public async Task DeleteObsoleteAsync()
        {
            var savedItems = await storageConnector.GetSavedItemsAsync();
            await storageConnector.DeleteObsoleteItemsAsync(savedItems);
        }
    }
}

