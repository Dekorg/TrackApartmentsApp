using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TrackApartments.Kufar.Domain.Connector.DTOs
{
    internal sealed class KufarDetails
    {
        [JsonProperty("result")]
        public KufarDetailsPartial Result { get; set; }
    }
}
