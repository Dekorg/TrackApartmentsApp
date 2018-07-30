using Newtonsoft.Json;

namespace TrackApartmentsApp.Domain.Connectors.OnlinerConnector.DTOs
{
    public class OnlinerConvertedPriceDetails
    {
        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
