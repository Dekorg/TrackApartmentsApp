using System;
using System.Threading.Tasks;
using TrackApartmentsApp.Core.Contracts.Storage;
using TrackApartmentsApp.Data.Storage.Entity.Extensions;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Data.Storage
{
    public class StorageAppartmentWriteRepository : IStorageWriteRepository<Apartment>
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
