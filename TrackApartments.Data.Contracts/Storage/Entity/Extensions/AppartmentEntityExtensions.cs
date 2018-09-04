using System;
using System.Globalization;
using System.Linq;
using System.Text;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Data.Contracts.Storage.Entity.Extensions
{
    public static class ApartmentExtensions
    {
        public static ApartmentEntity ToEntity(this Apartment apartment, string partitionKey, Guid rowKey)
        {
            var entity = new ApartmentEntity();
            entity.UniqueId = entity.UniqueId == Guid.Empty ? rowKey : apartment.UniqueId;
            entity.Address = apartment.Address;
            entity.Created = apartment.Created;
            entity.Updated = apartment.Updated;
            entity.IsCreatedByOwner = apartment.IsCreatedByOwner;
            entity.Price = apartment.Price.ToString(CultureInfo.InvariantCulture);
            entity.Rooms = apartment.Rooms;
            entity.Uri = apartment.Uri.AbsoluteUri;

            var sb = new StringBuilder();
            foreach (string phone in apartment.Phones)
            {
                sb.Append(phone);
                sb.Append(';');
            }

            entity.Phones = sb.ToString();
            entity.PartitionKey = partitionKey;
            entity.RowKey = rowKey.ToString();

            return entity;
        }

        public static Apartment ToApartment(this ApartmentEntity entity)
        {
            var apartment = new Apartment
            {
                UniqueId = entity.UniqueId == Guid.Empty ? Guid.Parse(entity.RowKey) : entity.UniqueId,
                Address = entity.Address,
                Created = entity.Created ?? DateTime.MinValue,
                Updated = entity.Updated ?? DateTime.MinValue,
                IsCreatedByOwner = entity.IsCreatedByOwner,
                Price = float.Parse(entity.Price),
                Rooms = entity.Rooms,
                Uri = new Uri(entity.Uri),
                Phones = entity.Phones.Split(';')
                    .Where(x => !String.IsNullOrEmpty(x.Trim())).ToList()
            };

            return apartment;
        }
    }
}
