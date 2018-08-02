using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.Storage.Domain.Contracts;
using TrackApartments.Storage.Settings;

namespace TrackApartments.Storage
{
    public sealed class StorageConnector : IStorageConnector
    {
        private readonly StorageSettings settings;
        private readonly IStorageReadRepository<Apartment> reader;
        private readonly IStorageWriteRepository<Apartment> writer;

        public StorageConnector(StorageSettings settings, IStorageWriteRepository<Apartment> writer, IStorageReadRepository<Apartment> reader)
        {
            this.settings = settings;
            this.reader = reader;
            this.writer = writer;
        }

        public bool IsObsoleteItem(Apartment apartment)
        {
            return (DateTime.Now - apartment.Created).Days > settings.StoreForPeriodInDays;
        }

        public async Task<List<Apartment>> GetSavedItemsAsync()
        {
            var savedItems = await reader.LoadAsync(settings.PartitionKey); // todo: to hashset
            return savedItems;
        }

        public async Task DeleteObsoleteItemsAsync(List<Apartment> savedItems)
        {
            foreach (var item in savedItems)
            {
                if (item != null && IsObsoleteItem(item))
                {
                    await writer.DeleteAsync(settings.PartitionKey, item);
                }
            }
        }

        public async Task SaveItemAsync(Apartment newItem)
        {
            await writer.SaveAsync(settings.PartitionKey, newItem);
        }
    }
}
