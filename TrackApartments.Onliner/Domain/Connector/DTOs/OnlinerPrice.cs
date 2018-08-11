using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    internal class OnlinerPrice
    {
        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("converted")]
        public OnlinerConvertedPrice Converted { get; set; }
    }
}
