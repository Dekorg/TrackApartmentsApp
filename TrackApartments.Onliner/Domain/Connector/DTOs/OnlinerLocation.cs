using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    internal class OnlinerLocation
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("user_address")]
        public string UserAddress { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}
