using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace TrackApartments.Data.Contracts.Storage.Entity
{
    public class ApartmentEntity : TableEntity
    {
        public Guid UniqueId { get; set; }

        public string Address { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        public string Phones { get; set; }

        public bool IsCreatedByOwner { get; set; }

        public string Price { get; set; }

        public int Rooms { get; set; }

        public string Uri { get; set; }
    }
}
