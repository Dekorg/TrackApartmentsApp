using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Kufar.Domain.Connector;

namespace TrackApartments.Kufar
{
    public class KufarApartmentService
    {
        private readonly IKufarConnector kufarConnector;
        private readonly ILogger logger;

        public KufarApartmentService(IKufarConnector kufarConnector, ILogger logger)
        {
            this.kufarConnector = kufarConnector;
            this.logger = logger;
        }

        public async Task<List<Apartment>> GetAppartments(string url)
        {
            var results = await GetKufarApartments(url);
            return results;
        }

        private async Task<List<Apartment>> GetKufarApartments(string url)
        {
            var results = await kufarConnector.GetAsync(url);

            foreach (var item in results)
            {
                logger.LogDebug($"Kufar item: {item.Address}, url: {item.Uri}");
            }

            return results;
        }
    }
}
