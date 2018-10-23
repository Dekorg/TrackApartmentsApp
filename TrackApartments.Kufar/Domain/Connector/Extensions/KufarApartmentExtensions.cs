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
        public static Apartment ToApartment(this KufarApartment kufarApartment, KufarDetailsPartial detailsPartial = null)
        {
            var apartment = new Apartment
            {
                Address = kufarApartment.Address,
                Created = kufarApartment.ListTime,
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
                    apartment.Phones = new List<string> { detailsPartial.Phone };
                }
            }

            if (Single.TryParse(kufarApartment.PriceUSD, out float price) && price > 0)
            {
                apartment.Price = price / 100;
            }

            apartment.SourceId = kufarApartment.Id.ToString();
            apartment.Rooms = kufarApartment.Rooms;
            apartment.Updated = kufarApartment.ListTime;
            apartment.Uri = new Uri($"https://re.kufar.by/vi/{kufarApartment.Id}");
            apartment.Source = DataSource.Kufar;

            return apartment;
        }
    }
}
