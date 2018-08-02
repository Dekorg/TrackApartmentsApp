using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts;
using TrackApartments.Contracts.Models;
using TrackApartments.Onliner.Domain.Connector;

namespace TrackApartments.Onliner
{
    public class OnlinerApartmentService
    {
        private readonly IOnlinerConnector onlinerConnector;
        private readonly IQueueWriter<Apartment> queueWriter;
        private readonly ILogger logger;

        public OnlinerApartmentService(IOnlinerConnector onlinerConnector, IQueueWriter<Apartment> queueWriter, ILogger logger)
        {
            this.onlinerConnector = onlinerConnector;
            this.queueWriter = queueWriter;
            this.logger = logger;
        }

        public async Task<List<Apartment>> GetAppartments(string url)
        {
            var results = await GetOnlinerApartments(url);
            return results;
        }

        private async Task<List<Apartment>> GetOnlinerApartments(string url)
        {
            var results = await onlinerConnector.GetAsync(url);

            foreach (var item in results)
            {
                logger.LogDebug($"Onliner item: {item.Address}, url: {item.Uri}");
                await queueWriter.WriteAsync(item);
            }

            return results;
        }
    }
}
