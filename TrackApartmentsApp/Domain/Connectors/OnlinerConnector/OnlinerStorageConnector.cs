using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackApartmentsApp.Core.Interfaces.Storage;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Domain.Connectors.Abstract;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Domain.Connectors.OnlinerConnector
{
    public class OnlinerStorageConnector : IStorageConnector
    {
        private readonly StorageSettings settings;
        private readonly IStorageReadRepository<Apartment> reader;
        private readonly IStorageWriteRepository<Apartment> writer;

        public OnlinerStorageConnector(StorageSettings settings, IStorageWriteRepository<Apartment> writer, IStorageReadRepository<Apartment> reader)
        {
            this.settings = settings;
            this.reader = reader;
            this.writer = writer;
        }

        public async Task<List<Apartment>> MergeAndGetNewAsync(IEnumerable<Apartment> newItems)
        {
            var savedItems = await reader.LoadAsync(settings.PartitionKey); // todo: to hashset

            foreach (var item in savedItems)
            {
                if (item != null)
                {
                    var period = DateTime.Now - item.Created;
                    if (period.Days > settings.StoreForPeriodInDays)
                    {
                        await writer.DeleteAsync(settings.PartitionKey, item);
                    }
                }
            }

            var newlySavedItems = new List<Apartment>();

            foreach (var newItem in newItems)
            {
                var period = DateTime.Now - newItem.Created;

                if (!savedItems.Contains(newItem) && period.Days <= settings.StoreForPeriodInDays)
                {
                    await writer.SaveAsync(settings.PartitionKey, newItem);
                    newlySavedItems.Add(newItem);
                }
            }

            return newlySavedItems;
        }
    }
}
