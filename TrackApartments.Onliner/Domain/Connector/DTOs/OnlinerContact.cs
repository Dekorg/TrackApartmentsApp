using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    internal class OnlinerContact
    {
        [JsonProperty("owner")]
        public bool IsOwner { get; set; }
    }
}
