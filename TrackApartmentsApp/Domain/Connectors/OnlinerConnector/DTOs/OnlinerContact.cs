using Newtonsoft.Json;

namespace TrackApartmentsApp.Domain.Connectors.OnlinerConnector.DTOs
{
    public class OnlinerContact
    {
        [JsonProperty("owner")]
        public bool IsOwner { get; set; }
    }
}
