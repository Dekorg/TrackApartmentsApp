using System.Net.Http;
using System.Threading.Tasks;

namespace TrackApartmentsApp.Core.Interfaces
{
    public interface IResponseParser
    {
        Task<T> ParseAsync<T>(HttpResponseMessage message);

        Task<string> GetContentAsync(HttpResponseMessage message);
    }
}
