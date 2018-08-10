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
            Queue.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }
    }
}
