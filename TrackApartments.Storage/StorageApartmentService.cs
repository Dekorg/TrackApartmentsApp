using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Storage.Domain.Contracts;

namespace TrackApartments.Storage
{
    public class StorageApartmentService
    {
        private readonly IStorageConnector storageConnector;
        private readonly ILogger logger;

        public StorageApartmentService(IStorageConnector storageConnector, ILogger logger)
        {
            this.storageConnector = storageConnector;
            this.logger = logger;
        }

        public async Task<bool> IsNewItemAsync(Apartment apartment)
        {
            if (apartment == null)
            {
                throw new NullReferenceException(nameof(apartment));
            }

            var savedItems = await storageConnector.GetSavedItemsAsync();

            bool isNew = !savedItems.Contains(apartment)
                && !storageConnector.IsObsoleteItem(apartment);

            if (isNew)
            {
                await storageConnector.SaveItemAsync(apartment);
                logger.LogDebug($"New apartment: {apartment.Address} url: {apartment.Uri}");
            }

            //todo: move to another scheduled service.
            // await storageConnector.DeleteObsoleteItemsAsync(savedItems);

            return isNew;
        }
    }
}
