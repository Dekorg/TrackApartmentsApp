using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TrackApartmentsApp.Core.Contracts;

namespace TrackApartmentsApp.Data
{
    public class LoadEngine : ILoadEngine
    {
        private readonly HttpClient client;

        public LoadEngine(HttpClient client)
        {
            this.client = client;
        }

        public async Task<HttpResponseMessage> LoadAsync(string url)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.GetAsync(url);
        }
    }
}
