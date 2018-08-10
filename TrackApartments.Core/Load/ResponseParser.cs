using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrackApartments.Contracts;

namespace TrackApartments.Core.Load
{
    public class ResponseParser : IResponseParser
    {
        public async Task<string> GetContentAsync(HttpResponseMessage message)
        {
            var receiveStream = await message.Content.ReadAsStreamAsync();
            var readStream = new StreamReader(receiveStream, Encoding.UTF8);
            return readStream.ReadToEnd();
        }

        public virtual async Task<T> ParseAsync<T>(HttpResponseMessage message)
        {
            var serializedContent = await message.Content.ReadAsStringAsync();
            var deserializedContent = JsonConvert.DeserializeObject<T>(
                serializedContent,
                new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
            return deserializedContent;
        }
    }
}
