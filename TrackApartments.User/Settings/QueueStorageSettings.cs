namespace TrackApartments.User.Settings
{
    public class QueueStorageSettings
    {
        public string ConnectionString { get; set; }

        public string NewOrdersSmsQueueName { get; set; }

        public string NewOrdersEmailQueueName { get; set; }
    }
}
