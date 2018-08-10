using System;
using System.Linq;
using System.Text;
using TrackApartments.Contracts.Models;

namespace TrackApartments.Storage.Domain.Storage.Entity.Extensions
{
    public static class ApartmentExtensions
    {
        public static ApartmentEntity ToEntity(this Apartment apartment, string partitionKey, string rowKey)
        {
            var entity = new ApartmentEntity();

            entity.Address = apartment.Address;
            entity.Created = apartment.Created;
            entity.Updated = apartment.Updated;
            entity.IsCreatedByOwner = apartment.IsCreatedByOwner;
            entity.Price = apartment.Price.ToString();
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
            entity.RowKey = rowKey;

            return entity;
        }

        public static Apartment ToApartment(this ApartmentEntity entity)
        {
            var apartment = new Apartment
            {
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
