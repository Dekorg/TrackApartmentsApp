﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TrackApartments.Contracts;
using TrackApartments.Contracts.Models;
using TrackApartments.Kufar.Domain.Connector.DTOs;
using TrackApartments.Kufar.Domain.Connector.Extensions;

namespace TrackApartments.Kufar.Domain.Connector
{
    public sealed class KufarConnector : IKufarConnector
    {
        private readonly ILoadEngine engine;
        private readonly IResponseParser parser;

        public KufarConnector(ILoadEngine engine, IResponseParser parser)
        {
            this.engine = engine;
            this.parser = parser;
        }

        public async Task<List<Apartment>> GetAsync(string url)
        {
            var data = await engine.LoadAsync(url);
            var kufarApartments = await parser.ParseAsync<KufarBoard>(data);

            var results = new List<Apartment>();

            foreach (var flat in kufarApartments.Apartments)
            {
                var detailsUrl = "https://re.kufar.by/api/search/detail/" + flat.Id;
                var detailsData = await engine.LoadAsync(detailsUrl);
                var details = await parser.ParseAsync<KufarDetails>(detailsData);

                results.Add(flat.ToAppartment(details.Result));
            }

            return results;
        }
    }
}