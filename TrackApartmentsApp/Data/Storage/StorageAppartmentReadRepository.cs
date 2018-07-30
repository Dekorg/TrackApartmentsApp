using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackApartmentsApp.Core.Interfaces.Storage;
using TrackApartmentsApp.Data.Storage.Entity;
using TrackApartmentsApp.Data.Storage.Entity.Extensions;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Data.Storage
{
    public class StorageAppartmentReadRepository : IStorageReadRepository<Apartment>
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
