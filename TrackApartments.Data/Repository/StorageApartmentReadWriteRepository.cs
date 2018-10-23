using System;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.Data.Contracts;
using TrackApartments.Data.Contracts.Storage.Entity;

namespace TrackApartments.Data.Repository
{
    public class StorageApartmentReadWriteRepository : IStorageReadWriteRepository<Apartment>
    {
        private readonly IStorageWorker worker;

        public StorageApartmentReadWriteRepository(IStorageWorker worker)
        {
            this.worker = worker;
        }

        public async Task DeleteAsync(string partitionKey, string rowKey)
        {
            var item = await worker.LoadAsync<ApartmentEntity>(partitionKey, rowKey);
            await worker.DeleteAsync(item);
        }
    }
}
