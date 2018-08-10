using System;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.Storage.Domain.Contracts;
using TrackApartments.Storage.Domain.Storage.Entity.Extensions;

namespace TrackApartments.Storage.Domain.Storage
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
            var entity = item.ToEntity(partitionKey, Guid.NewGuid().ToString());
            await worker.SaveAsync(entity);
        }

        public async Task DeleteAsync(string partitionKey, Apartment item)
        {
            var entity = item.ToEntity(partitionKey, Guid.NewGuid().ToString());
            await worker.DeleteAsync(entity);
        }
    }
}
