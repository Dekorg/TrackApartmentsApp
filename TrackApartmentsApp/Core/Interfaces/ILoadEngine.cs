using System.Net.Http;
using System.Threading.Tasks;

namespace TrackApartmentsApp.Core.Interfaces
{
    public interface ILoadEngine
    {
        Task<HttpResponseMessage> LoadAsync(string url);
    }
}
