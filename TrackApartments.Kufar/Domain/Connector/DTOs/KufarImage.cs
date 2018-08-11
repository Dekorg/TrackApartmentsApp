using System;
using Newtonsoft.Json;

namespace TrackApartments.Kufar.Domain.Connector.DTOs
{
    internal sealed class KufarImage
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
