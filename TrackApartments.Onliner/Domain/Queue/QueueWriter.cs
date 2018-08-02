using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using TrackApartments.Contracts;
using TrackApartments.Contracts.Models;
using TrackApartments.Onliner.Settings;

namespace TrackApartments.Onliner.Domain.Queue
{
    public class QueueWriter : IQueueWriter<Apartment>
    {
        private CloudQueue queue;

        public QueueWriter(QueueStorageSettings queueStorageSettings)
        {
            var storageAccount = CloudStorageAccount.Parse(queueStorageSettings.ConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            queue = queueClient.GetQueueReference(queueStorageSettings.QueueName);
            queue.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

        public async Task WriteAsync(Apartment apartment)
        {
            var messageAsJson = JsonConvert.SerializeObject(apartment);
            var message = new CloudQueueMessage(messageAsJson);
            await queue.AddMessageAsync(message);
        }
    }
}
