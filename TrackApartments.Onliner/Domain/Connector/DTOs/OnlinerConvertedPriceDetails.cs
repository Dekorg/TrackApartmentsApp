using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    internal class OnlinerConvertedPriceDetails
    {
        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
