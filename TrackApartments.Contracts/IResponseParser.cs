using System.Net.Http;
using System.Threading.Tasks;

namespace TrackApartments.Contracts
{
    public interface IResponseParser
    {
        Task<T> ParseAsync<T>(HttpResponseMessage message);

        Task<string> GetContentAsync(HttpResponseMessage message);
    }
}
