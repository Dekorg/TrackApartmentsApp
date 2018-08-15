using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Get.Domain.Contracts
{
    public interface IStorageConnector
    {
        Task<List<Apartment>> GetItemsList();
    }
}
