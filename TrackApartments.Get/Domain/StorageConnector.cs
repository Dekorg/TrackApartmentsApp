using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.Data.Contracts;
using TrackApartments.Data.Contracts.Settings;
using TrackApartments.Get.Domain.Contracts;

namespace TrackApartments.Get.Domain
{
    public sealed class StorageConnector : IStorageConnector
    {
        private readonly StorageSettings settings;
        private readonly IStorageReadRepository<Apartment> reader;

        public StorageConnector(StorageSettings settings, IStorageReadRepository<Apartment> reader)
        {
            this.settings = settings;
            this.reader = reader;
        }

        public async Task<List<Apartment>> GetItemsList()
        {
            return await reader.LoadAsync(settings.PartitionKey);
        }
    }
}
