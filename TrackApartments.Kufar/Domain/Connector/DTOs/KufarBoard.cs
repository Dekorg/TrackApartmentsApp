using System.Collections.Generic;
using Newtonsoft.Json;

namespace TrackApartments.Kufar.Domain.Connector.DTOs
{
    internal sealed class KufarBoard
    {
        [JsonProperty("ads")]
        public List<KufarApartment> Apartments { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
