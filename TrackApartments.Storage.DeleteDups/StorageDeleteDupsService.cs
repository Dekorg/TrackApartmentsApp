using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.Storage.DeleteDups.Domain.Comparers.Predicate;
using TrackApartments.Storage.DeleteDups.Domain.Contracts;

namespace TrackApartments.Storage.DeleteDups
{
    public class StorageDeleteDupsService
    {
        private readonly IStorageConnector storageConnector;

        public StorageDeleteDupsService(IStorageConnector storageConnector)
        {
            this.storageConnector = storageConnector;
        }

        public async Task DeleteDupsAsync()
        {
            var savedItems = await storageConnector.GetSavedItemsAsync();

            var duplicates = new List<Apartment>();

            for (var index = 0; index < savedItems.Count; index++)
            {
                var item = savedItems[index];
                savedItems[index] = null;
                if (savedItems.Exists(apartment =>
                    new ApartmentEqualityPredicate()
                        .CompositePredicate(apartment, item)))
                {
                    duplicates.Add(item);
                }
                else
                {
                    savedItems[index] = item;
                }
            }

            await storageConnector.DeleteItems(duplicates);
        }
    }
}

