using System;
using Newtonsoft.Json;

namespace TrackApartments.Onliner.Domain.Connector.DTOs
{
    public class OnlinerApartment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public OnlinerPrice Price { get; set; }

        [JsonProperty("rent_type")]
        public string RentType { get; set; }

        [JsonProperty("location")]
        public OnlinerLocation Location { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("contact")]
        public OnlinerContact Contact { get; set; }

        [JsonProperty("created_at")]
        public DateTime Created { get; set; }

        [JsonProperty("last_time_up")]
        public DateTime Updated { get; set; }

        [JsonProperty("up_available_in")]
        public int UpAvailableIn { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
