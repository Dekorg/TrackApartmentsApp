using System;
using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Queue.Contracts;
using TrackApartments.User.Settings;

namespace TrackApartments.User.Domain.Queue
{
    public class SmsQueueWriter : QueueWriter, ISmsQueueWriter<Order>
    {
        public SmsQueueWriter(QueueStorageSettings queueStorageSettings)
            : base(queueStorageSettings)
        {
            Queue = QueueClient.GetQueueReference(queueStorageSettings.NewOrdersSmsQueueName);
            if (!Queue.ExistsAsync().GetAwaiter().GetResult())
            {
                Queue.CreateIfNotExistsAsync().GetAwaiter().GetResult();
            }
        }
    }
}
