using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TrackApartments.Contracts.Enums;
using TrackApartments.Contracts.Models;
using TrackApartments.Contracts.Regexps;
using TrackApartments.Kufar.Domain.Connector.DTOs;

namespace TrackApartments.Kufar.Domain.Connector.Extensions
{
    internal static class KufarApartmentExtensions
    {
        public static Apartment ToAppartment(this KufarApartment kufarAppartment, KufarDetailsPartial detailsPartial = null)
        {
            var appartment = new Apartment
            {
                Address = kufarAppartment.Address,
                Created = kufarAppartment.ListTime,
                IsCreatedByOwner = detailsPartial?.IsCompanyAd == 0
            };

            if (!String.IsNullOrEmpty(detailsPartial?.Phone))
            {
                if (!detailsPartial.Phone.StartsWith("+"))
                {
                    detailsPartial.Phone = "+" + detailsPartial.Phone;
                }

                if (new PhoneRegex().Expression.IsMatch(detailsPartial.Phone))
                {
                    appartment.Phones = new List<string> { detailsPartial.Phone };
                }
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
