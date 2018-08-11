using System;
using Newtonsoft.Json;

namespace TrackApartments.Kufar.Domain.Connector.DTOs
{
    internal sealed class KufarDetailsPartial
    {
        [JsonProperty("ad_id")]
        public int Id { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("company_ad")]
        public int IsCompanyAd { get; set; }
    }
}
