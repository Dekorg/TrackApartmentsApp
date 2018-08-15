using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.Data.Contracts;
using TrackApartments.Data.Contracts.Storage.Entity;
using TrackApartments.Data.Contracts.Storage.Entity.Extensions;

namespace TrackApartments.Data.Repository
{
    public sealed class StorageAppartmentReadRepository : IStorageReadRepository<Apartment>
    {
        private readonly IStorageWorker worker;

        public StorageAppartmentReadRepository(IStorageWorker worker)
        {
            this.worker = worker;
        }

        public async Task<List<Apartment>> LoadAsync(string partitionKey)
        {
            var items = await worker.LoadListAsync<ApartmentEntity>(partitionKey);
            return items.Select(x => x.ToApartment()).ToList();
        }
    }
}
