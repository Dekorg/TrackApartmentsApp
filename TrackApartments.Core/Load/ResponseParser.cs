using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackApartments.Contracts;

namespace TrackApartments.Core.Load
{
    public class ResponseParser : IResponseParser
    {
        private readonly ILogger logger;

        public ResponseParser(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<string> GetContentAsync(HttpResponseMessage message)
        {
            var receiveStream = await message.Content.ReadAsStreamAsync();
            var readStream = new StreamReader(receiveStream, Encoding.UTF8);
            return readStream.ReadToEnd();
        }

        public virtual async Task<T> ParseAsync<T>(HttpResponseMessage message)
        {
            string serializedContent = string.Empty;

            try
            {
                serializedContent = await message.Content.ReadAsStringAsync();

                var deserializedContent = JsonConvert.DeserializeObject<T>(
                    serializedContent,
                    new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });

                if (deserializedContent == null)
                {
                    throw new ArgumentNullException(nameof(deserializedContent));
                }

                return deserializedContent;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error {ex} during page parsing: {message}, with raw content {serializedContent}", ex);
                throw;
            }
        }
    }
}
