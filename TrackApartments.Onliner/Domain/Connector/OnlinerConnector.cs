using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackApartments.Contracts;
using TrackApartments.Contracts.Models;
using TrackApartments.Contracts.PageParser;
using TrackApartments.Contracts.Regexps;
using TrackApartments.Onliner.Domain.Connector.DTOs;
using TrackApartments.Onliner.Domain.Connector.Extensions;

namespace TrackApartments.Onliner.Domain.Connector
{
    public sealed class OnlinerConnector : IOnlinerConnector
    {
        private readonly ILoadEngine engine;
        private readonly IResponseParser parser;
        private readonly IPageParser pageParser;

        public OnlinerConnector(ILoadEngine engine, IResponseParser parser, IOnlinerPageParser pageParser)
        {
            this.engine = engine;
            this.parser = parser;
            this.pageParser = pageParser;
        }

        public async Task<List<Apartment>> GetAsync(string url)
        {
            var data = await engine.LoadAsync(url);
            var parsed = await parser.ParseAsync<OnlinerBoard>(data);
            var appartments = parsed.Apartments.Select(x => x.ToAppartment()).ToList();

            foreach (var flat in appartments)
            {
                var response = await engine.LoadAsync(flat.Uri.AbsoluteUri);
                var content = await parser.GetContentAsync(response);

                flat.Phones = pageParser.FindByRegex(content, new PhoneRegex().Expression).ToList();
            }

            return appartments;
        }
    }
}
