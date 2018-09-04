using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TrackApartments.Contracts;

namespace TrackApartments.Core.Load
{
    public class LoadEngine : ILoadEngine
    {
        private readonly HttpClient client;

        public LoadEngine(HttpClient client)
        {
            this.client = client;
        }

        public virtual async Task<HttpResponseMessage> LoadAsync(string url)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.SendAsync(message);
        }
    }
}
