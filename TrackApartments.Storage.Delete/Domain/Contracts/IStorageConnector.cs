using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Storage.Delete.Domain.Contracts
{
    public interface IStorageConnector
    {
        Task<List<Apartment>> GetSavedItemsAsync();

        Task DeleteObsoleteItemsAsync(List<Apartment> savedItems);
    }
}
