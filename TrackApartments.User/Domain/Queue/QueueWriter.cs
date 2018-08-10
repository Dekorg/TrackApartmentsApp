using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using TrackApartments.Contracts;
using TrackApartments.Contracts.Models;
using TrackApartments.User.Settings;

namespace TrackApartments.User.Domain.Queue
{
    public abstract class QueueWriter : IQueueWriter<Order>
    {
        protected CloudQueueClient QueueClient { get; }

        protected CloudQueue Queue { get; set; }

        protected QueueWriter(QueueStorageSettings queueStorageSettings)
        {
            var storageAccount = CloudStorageAccount.Parse(queueStorageSettings.ConnectionString);
            QueueClient = storageAccount.CreateCloudQueueClient();
        }

        public virtual async Task WriteAsync(Order order)
        {
            var messageAsJson = JsonConvert.SerializeObject(order);
            var message = new CloudQueueMessage(messageAsJson);
            await Queue.AddMessageAsync(message);
        }
    }
}
