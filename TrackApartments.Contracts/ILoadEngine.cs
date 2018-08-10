using System.Net.Http;
using System.Threading.Tasks;

namespace TrackApartments.Contracts
{
    public interface ILoadEngine
    {
        Task<HttpResponseMessage> LoadAsync(string url);
    }
}
