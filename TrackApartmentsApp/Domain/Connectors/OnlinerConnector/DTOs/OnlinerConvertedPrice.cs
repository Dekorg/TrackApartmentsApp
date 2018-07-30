using Newtonsoft.Json;

namespace TrackApartmentsApp.Domain.Connectors.OnlinerConnector.DTOs
{
    public class OnlinerConvertedPrice
    {
        [JsonProperty("USD")]
        public OnlinerConvertedPriceDetails USD { get; set; }

        [JsonProperty("BYN")]
        public OnlinerConvertedPriceDetails BYN { get; set; }
    }
}
