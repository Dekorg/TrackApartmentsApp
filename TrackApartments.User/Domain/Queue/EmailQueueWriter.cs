using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Queue.Contracts;
using TrackApartments.User.Settings;

namespace TrackApartments.User.Domain.Queue
{
    class EmailQueueWriter : QueueWriter, IEmailQueueWriter<Order>
    {
        public EmailQueueWriter(QueueStorageSettings queueStorageSettings)
            : base(queueStorageSettings)
        {
            Queue = QueueClient.GetQueueReference(queueStorageSettings.NewOrdersEmailQueueName);
            if (!Queue.ExistsAsync().GetAwaiter().GetResult())
            {
                Queue.CreateIfNotExistsAsync().GetAwaiter().GetResult();
            }
        }
    }
}
