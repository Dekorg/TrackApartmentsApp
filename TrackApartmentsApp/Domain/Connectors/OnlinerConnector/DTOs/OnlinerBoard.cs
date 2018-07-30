using System.Collections.Generic;
using Newtonsoft.Json;

namespace TrackApartmentsApp.Domain.Connectors.OnlinerConnector.DTOs
{
    public class OnlinerBoard
    {
        [JsonProperty("apartments")]
        public List<OnlinerApartment> Appartments { get; set; }
    }
}
