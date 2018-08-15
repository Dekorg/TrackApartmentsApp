namespace TrackApartments.Data.Contracts.Settings
{
    public class StorageSettings
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public int StoreForPeriodInDays { get; set; }

        public string PartitionKey { get; set; }
    }
}
