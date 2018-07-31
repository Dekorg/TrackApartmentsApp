using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Domain.Connectors.Abstract
{
    public interface IConnector
    {
        Task<List<Apartment>> GetAsync();
    }
}
