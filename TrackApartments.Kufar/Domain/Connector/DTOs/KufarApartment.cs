using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TrackApartments.Kufar.Domain.Connector.DTOs
{
    internal sealed class KufarApartment
    {
        [JsonProperty("ad_id")]
        public int Id { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("category")]
        public int Category { get; set; }

        [JsonProperty("coordinates")]
        public float[] Coordinates { get; set; }

        [JsonProperty("images")]
        public List<KufarImage> Images { get; set; }

        [JsonProperty("list_id")]
        public int ListId { get; set; }

        [JsonProperty("list_time")]
        public DateTime ListTime { get; set; }

        [JsonProperty("price_byn")]
        public string PriceBYN { get; set; }

        [JsonProperty("price_usd")]
        public string PriceUSD { get; set; }

        [JsonProperty("property_type")]
        public int PropertyType { get; set; }

        [JsonProperty("region")]
        public int Region { get; set; }

        [JsonProperty("remuneration_type")]
        public int RenumerationType { get; set; }

        [JsonProperty("rooms")]
        public int Rooms { get; set; }

        [JsonProperty("size")]
        public float Size { get; set; }

        [JsonProperty("size_area")]
        public float SizeArea { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("tmp_blocket_url")]
        public string Url { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

    }
}
