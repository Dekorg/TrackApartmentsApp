using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    internal class OnlinerConvertedPrice
    {
        [JsonProperty("USD")]
        public OnlinerConvertedPriceDetails USD { get; set; }

        [JsonProperty("BYN")]
        public OnlinerConvertedPriceDetails BYN { get; set; }
    }
}
