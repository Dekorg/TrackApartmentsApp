using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    public class OnlinerContact
    {
        [JsonProperty("owner")]
        public bool IsOwner { get; set; }
    }
}
