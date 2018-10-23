using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Data.Contracts;
using TrackApartments.Data.Contracts.Settings;
using TrackApartments.Storage.Delete.Domain.Contracts;

namespace TrackApartments.Storage.Delete.Domain
{
    public sealed class StorageConnector : IStorageConnector
    {
        private readonly StorageSettings settings;
        private readonly IStorageReadRepository<Apartment> reader;
        private readonly ILogger logger;
        private readonly IStorageReadWriteRepository<Apartment> readWriter;

        public StorageConnector(StorageSettings settings,
            IStorageReadWriteRepository<Apartment> readWriter,
            IStorageReadRepository<Apartment> reader,
            ILogger logger)
        {
            this.settings = settings;
            this.reader = reader;
            this.logger = logger;
            this.readWriter = readWriter;
        }

        public bool IsObsoleteItem(Apartment apartment)
        {
            return (DateTime.Now - apartment.Created).Days > settings.StoreForPeriodInDays;
        }

        public async Task<List<Apartment>> GetSavedItemsAsync()
        {
            var savedItems = await reader.LoadAsync(settings.PartitionKey);
            return savedItems;
        }

        public async Task DeleteObsoleteItemsAsync(List<Apartment> savedItems)
        {
            foreach (var item in savedItems)
            {
                if (item != null && IsObsoleteItem(item))
                {
                    try
                    {
                        await readWriter.DeleteAsync(settings.PartitionKey, item.UniqueId.ToString());
                        logger.LogWarning($"Apartment is obsolete and has to be disintegrated: {item.Address} url: {item.Uri}", item);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Failed to delete item : {item.Address}, url: {item.Uri}", item);
                    }
                }
            }
        }
    }
}
