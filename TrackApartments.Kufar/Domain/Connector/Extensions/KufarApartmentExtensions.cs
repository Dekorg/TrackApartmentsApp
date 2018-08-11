using System;
using System.Collections.Generic;
using TrackApartments.Contracts.Enums;
using TrackApartments.Contracts.Models;
using TrackApartments.Kufar.Domain.Connector.DTOs;

namespace TrackApartments.Kufar.Domain.Connector.Extensions
{
    internal static class KufarApartmentExtensions
    {
        public static Apartment ToAppartment(this KufarApartment kufarAppartment, KufarDetailsPartial detailsPartial)
        {
            var appartment = new Apartment
            {
                Address = kufarAppartment.Address,
                Created = kufarAppartment.ListTime,
                IsCreatedByOwner = detailsPartial.IsCompanyAd == 0
            };

            if (!String.IsNullOrEmpty(detailsPartial.Phone))
            {
                appartment.Phones = new List<string> { detailsPartial.Phone };
            }

            if (Single.TryParse(kufarAppartment.PriceUSD, out float price))
            {
                appartment.Price = price;
            }

            appartment.Rooms = kufarAppartment.Rooms;
            appartment.Updated = kufarAppartment.ListTime;
            appartment.Uri = new Uri(kufarAppartment.Url);
            appartment.Source = DataSource.Kufar;

            return appartment;
        }
    }
}
