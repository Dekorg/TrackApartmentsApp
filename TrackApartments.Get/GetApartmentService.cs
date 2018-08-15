using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Get.Domain.Contracts;

namespace TrackApartments.Get
{
    public class GetApartmentService
    {
        private readonly IStorageConnector storageConnector;
        private readonly ILogger logger;

        public GetApartmentService(IStorageConnector storageConnector, ILogger logger)
        {
            this.storageConnector = storageConnector;
            this.logger = logger;
        }

        public async Task<List<Apartment>> GetItemsAsync()
        {
            return await storageConnector.GetItemsList();
        }

        public async Task<Apartment> GetItemAsync(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
