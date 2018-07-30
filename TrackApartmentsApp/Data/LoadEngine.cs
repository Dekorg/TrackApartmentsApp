using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TrackApartmentsApp.Core.Interfaces;

namespace TrackApartmentsApp.Data
{
    public class LoadEngine : ILoadEngine
    {
        public async Task<HttpResponseMessage> LoadAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return await httpClient.GetAsync(url);
            }
        }
    }
}
