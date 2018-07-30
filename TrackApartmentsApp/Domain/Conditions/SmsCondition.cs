using System;
using System.Collections.Generic;
using TrackApartmentsApp.Domain.Models;

namespace TrackApartmentsApp.Domain.Conditions
{
    public class SmsCondition
    {
        public IEnumerable<Apartment> GetValid(List<Apartment> list)
        {
            foreach (var item in list)
            {
                var deltaTime = (int)DateTime.Now.Subtract(item.Created).TotalDays;

                if (item.IsCreatedByOwner && deltaTime <= 1 && item.Price <= 350)
                {
                    yield return item;
                }
            }
        }
    }
}
