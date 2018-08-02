using System.Collections.Generic;
using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    public class OnlinerBoard
    {
        [JsonProperty("apartments")]
        public List<OnlinerApartment> Appartments { get; set; }
    }
}
