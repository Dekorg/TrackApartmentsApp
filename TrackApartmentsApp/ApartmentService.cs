using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TrackApartmentsApp.Domain.Conditions;
using TrackApartmentsApp.Domain.Connectors.Abstract;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Sinks.Abstract;

namespace TrackApartmentsApp
{
    public class ApartmentService
    {
        private readonly IStorageConnector storageConnector;
        private readonly IOnlinerConnector onlinerConnector;
        private readonly ICompositeSink<Apartment> sink;
        private readonly ILogger logger;

        public ApartmentService(IStorageConnector storageConnector, IOnlinerConnector onlinerConnector, ICompositeSink<Apartment> sink, ILogger logger)
        {
            this.storageConnector = storageConnector;
            this.onlinerConnector = onlinerConnector;
            this.sink = sink;
            this.logger = logger;
        }

        public async Task StartAsync(ExecutionContext context)
        {
            var results = await GetOnlinerApartments(onlinerConnector);
            var items = await GetNewlyCreatedItemsAsync(results);
            items.ForEach(async x => await sink.WriteAsync(x));
        }

        private async Task<List<Apartment>> GetOnlinerApartments(IOnlinerConnector connector)
        {
            var results = await connector.GetAsync();

            foreach (var item in results)
            {
                logger.LogDebug($"All items: {item.Address}, url: {item.Uri}");
            }

            return results;
        }

        private async Task<List<Apartment>> GetNewlyCreatedItemsAsync(List<Apartment> results)
        {
            var newItems = await storageConnector.MergeAndGetNewAsync(results);

            logger.LogDebug($"Count new items: {newItems.Count}");

            SmsCondition condition = new SmsCondition();
            var validResults = condition.GetValid(newItems).ToList();

            foreach (var item in validResults)
            {
                logger.LogDebug($"New items: {item.Address} url: {item.Uri}");
            }

            return validResults;
        }
    }
}
