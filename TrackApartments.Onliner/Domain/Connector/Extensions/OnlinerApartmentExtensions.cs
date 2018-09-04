using System;
using System.Text.RegularExpressions;
using TrackApartments.Contracts.Enums;
using TrackApartments.Contracts.Models;
using TrackApartments.Onliner.Domain.Connector.DTOs;

namespace TrackApartments.Onliner.Domain.Connector.Extensions
{
    internal static class OnlinerApartmentExtensions
    {
        public const int MinimalSupposedLocationNameLength = 3;

        public static Apartment ToApartment(this OnlinerApartment onlinerApartment)
        {
            var apartment = new Apartment();

            if (String.IsNullOrEmpty(onlinerApartment.Location.Address) ||
                onlinerApartment.Location.Address.Length <= MinimalSupposedLocationNameLength)
            {
                onlinerApartment.Location.Address = onlinerApartment.Location.UserAddress;
            }

            apartment.Address = onlinerApartment.Location.Address;
            apartment.Created = onlinerApartment.Created;
            apartment.Updated = onlinerApartment.Updated;
            apartment.SourceId = onlinerApartment.Id.ToString();
            apartment.IsCreatedByOwner = onlinerApartment.Contact.IsOwner;
            apartment.Price = onlinerApartment.Price.Converted.USD.Amount;
            apartment.Rooms = Int32.Parse(Regex.Match(onlinerApartment.RentType, @"\d+").Value);
            apartment.Uri = new Uri(onlinerApartment.Url);
            apartment.Source = DataSource.Onliner;

            return apartment;
        }
    }
}
