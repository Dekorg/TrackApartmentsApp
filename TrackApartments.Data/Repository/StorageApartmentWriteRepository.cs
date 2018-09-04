using System;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.Data.Contracts;
using TrackApartments.Data.Contracts.Storage.Entity.Extensions;

namespace TrackApartments.Data.Repository
{
    public sealed class StorageApartmentWriteRepository : IStorageWriteRepository<Apartment>
    {
        private readonly IStorageWorker worker;

        public StorageApartmentWriteRepository(IStorageWorker worker)
        {
            this.worker = worker;
        }

        public async Task SaveAsync(string partitionKey, Apartment item)
        {
            var entity = item.ToEntity(partitionKey, Guid.NewGuid());
            await worker.SaveAsync(entity);
        }
    }
}
