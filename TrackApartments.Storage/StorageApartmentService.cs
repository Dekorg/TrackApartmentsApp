using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts;
using TrackApartments.Contracts.Models;
using TrackApartments.Storage.Domain.Contracts;

namespace TrackApartments.Storage
{
    public class StorageApartmentService
    {
        private readonly IStorageConnector storageConnector;
        private readonly IQueueWriter<Apartment> queueWriter;
        private readonly ILogger logger;

        public StorageApartmentService(IStorageConnector storageConnector, IQueueWriter<Apartment> queueWriter, ILogger logger)
        {
            this.storageConnector = storageConnector;
            this.queueWriter = queueWriter;
            this.logger = logger;
        }

        public async Task<bool> SaveIfNewItemAsync(Apartment apartment)
        {
            if (apartment == null)
            {
                throw new NullReferenceException(nameof(apartment));
            }

            var savedItems = await storageConnector.GetSavedItemsAsync();

            bool isNew = !savedItems.Contains(apartment) && !storageConnector.IsObsoleteItem(apartment);

            if (isNew)
            {
                await storageConnector.SaveItemAsync(apartment);
                await queueWriter.WriteAsync(apartment);

                logger.LogDebug($"New apartment: {apartment.Address} url: {apartment.Uri}");
            }

            //todo: move to another scheduled service.
            // await storageConnector.DeleteObsoleteItemsAsync(savedItems);

            return isNew;
        }
    }
}
