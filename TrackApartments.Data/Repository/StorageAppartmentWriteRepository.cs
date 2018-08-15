using System;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.Data.Contracts;
using TrackApartments.Data.Contracts.Storage.Entity.Extensions;

namespace TrackApartments.Data.Repository
{
    public sealed class StorageAppartmentWriteRepository : IStorageWriteRepository<Apartment>
    {
        private readonly IStorageWorker worker;

        public StorageAppartmentWriteRepository(IStorageWorker worker)
        {
            this.worker = worker;
        }

        public async Task SaveAsync(string partitionKey, Apartment item)
        {
            var entity = item.ToEntity(partitionKey, Guid.NewGuid());
            await worker.SaveAsync(entity);
        }

        public async Task DeleteAsync(string partitionKey, Apartment item)
        {
            var entity = item.ToEntity(partitionKey, Guid.NewGuid());
            await worker.DeleteAsync(entity);
        }
    }
}
