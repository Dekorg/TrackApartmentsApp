using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Contracts
{
    public interface IConnector
    {
        Task<List<Apartment>> GetAsync(string url);
    }
}
