using Newtonsoft.Json;

namespace TrackApartmentsApp.Domain.Connectors.OnlinerConnector.DTOs
{
    public class OnlinerPrice
    {
        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("converted")]
        public OnlinerConvertedPrice Converted { get; set; }
    }
}
