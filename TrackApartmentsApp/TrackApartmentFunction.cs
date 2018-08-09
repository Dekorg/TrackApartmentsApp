using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TrackApartmentsApp
{
    public static class TrackApartmentFunction
    {
        public static readonly HttpClient CachedClient = new HttpClient();

        //private const string Url =
        //       "https://trackapartmentsapp.azurewebsites.net/api/TrackOnlinerApartmentsFunction?url=https%3A%2F%2Fak.api.onliner.by%2Fsearch%2Fapartments%3Frent_type%5B%5D%3D1_room%26rent_type%5B%5D%3D2_rooms%26price%5Bmin%5D%3D50%26price%5Bmax%5D%3D350%26currency%3Dusd%26only_owner%3Dtrue%26metro%5B%5D%3Dblue_line%26bounds%5Blb%5D%5Blat%5D%3D53.880093907614935%26bounds%5Blb%5D%5Blong%5D%3D27.56574851112753%26bounds%5Brt%5D%5Blat%5D%3D53.944535109493444%26bounds%5Brt%5D%5Blong%5D%3D27.673566578482692%26_%3D0.07703344547452784";

        private const string Url =
             "http://localhost:7071/api/TrackOnlinerApartmentsFunction?url=https%3A%2F%2Fak.api.onliner.by%2Fsearch%2Fapartments%3Frent_type%5B%5D%3D1_room%26rent_type%5B%5D%3D2_rooms%26price%5Bmin%5D%3D50%26price%5Bmax%5D%3D350%26currency%3Dusd%26only_owner%3Dtrue%26metro%5B%5D%3Dblue_line%26bounds%5Blb%5D%5Blat%5D%3D53.880093907614935%26bounds%5Blb%5D%5Blong%5D%3D27.56574851112753%26bounds%5Brt%5D%5Blat%5D%3D53.944535109493444%26bounds%5Brt%5D%5Blong%5D%3D27.673566578482692%26_%3D0.07703344547452784";


        [FunctionName("TrackApartmentFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogDebug($"{nameof(TrackApartmentFunction)} has started.", myTimer);

            await CachedClient.GetAsync(Url);
        }
    }
}

