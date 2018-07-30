using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackApartmentsApp.Core.Interfaces;
using TrackApartmentsApp.Data.Regexps;
using TrackApartmentsApp.Domain.Connectors.Abstract;
using TrackApartmentsApp.Domain.Connectors.OnlinerConnector.DTOs;
using TrackApartmentsApp.Domain.Connectors.OnlinerConnector.Extensions;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Domain.Connectors.OnlinerConnector
{
    public class OnlinerConnector : IConnector
    {
        private readonly ILoadEngine engine;
        private readonly IResponseParser parser;
        private readonly IPageParser pageParser;

        public OnlinerConnector(ILoadEngine engine, IResponseParser parser, IPageParser pageParser)
        {
            this.engine = engine;
            this.parser = parser;
            this.pageParser = pageParser;
        }

        public async Task<List<Apartment>> GetAsync(string url)
        {
            var data = await engine.LoadAsync(url);
            var parsed = await parser.ParseAsync<OnlinerBoard>(data);
            var appartments = parsed.Appartments.Select(x => x.ToAppartment()).ToList();

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
