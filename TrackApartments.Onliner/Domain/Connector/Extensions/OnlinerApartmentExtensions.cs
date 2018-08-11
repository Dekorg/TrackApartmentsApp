using System;
using System.Text.RegularExpressions;
using TrackApartments.Contracts.Enums;
using TrackApartments.Contracts.Models;
using TrackApartments.Onliner.Domain.Connector.DTOs;

namespace TrackApartments.Onliner.Domain.Connector.Extensions
{
    internal static class OnlinerApartmentExtensions
    {
        public static Apartment ToAppartment(this OnlinerApartment onlinerAppartment)
        {
            var appartment = new Apartment();

            if (String.IsNullOrEmpty(onlinerAppartment.Location.Address) || onlinerAppartment.Location.Address.Length <= 5)
            {
                onlinerAppartment.Location.Address = onlinerAppartment.Location.UserAddress;
            }

            appartment.Address = onlinerAppartment.Location.Address;
            appartment.Created = onlinerAppartment.Created;
            appartment.Updated = onlinerAppartment.Updated;
            appartment.IsCreatedByOwner = onlinerAppartment.Contact.IsOwner;
            appartment.Price = onlinerAppartment.Price.Converted.USD.Amount;
            appartment.Rooms = Int32.Parse(Regex.Match(onlinerAppartment.RentType, @"\d+").Value);
            appartment.Uri = new Uri(onlinerAppartment.Url);
            appartment.Source = DataSource.Onliner;

            return appartment;
        }
    }
}
