using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Storage.DeleteDups.Domain.Contracts
{
    public interface IStorageConnector
    {
        Task<List<Apartment>> GetSavedItemsAsync();

        Task DeleteItems(List<Apartment> savedItems);
    }
}
