using System.Collections.Generic;
using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    internal class OnlinerBoard
    {
        [JsonProperty("apartments")]
        public List<OnlinerApartment> Apartments { get; set; }
    }
}
