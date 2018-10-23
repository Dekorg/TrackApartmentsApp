using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Storage.Domain.Contracts
{
    public interface IStorageConnector
    {
        Task<List<Apartment>> GetSavedItemsAsync();

        bool IsObsoleteItem(Apartment apartment);

        Task SaveItemAsync(Apartment newItem);
    }
}
