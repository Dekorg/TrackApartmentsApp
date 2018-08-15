

using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Sinks.Abstract;

namespace TrackApartments.User
{
    public class UserOrderService
    {
        private readonly ICompositeSink<Order> sink;

        public UserOrderService(ICompositeSink<Order> sink)
        {
            this.sink = sink;
        }

        public async Task SaveAsync(Apartment apartment)
        {
            var order = new Order
            {
                Apartment = apartment,
                UserInfo = new UserInfo
                {
                    UserName = "Vitaly Bibikov",
                    Phone = "+375291602219",
                    Email = "evilavenger@yandex.ru"
                },
                SinkSettings = new UserSinkSettings
                {
                    EmailSettings = new SinkSettings
                    {
                        DesiredPriceBorder = 400,
                        IsNewPeriod = 1,
                        IsOnlyOwner = false
                    },
                    SmsSettings = new SinkSettings
                    {
                        DesiredPriceBorder = 300,
                        IsNewPeriod = 1,
                        IsOnlyOwner = true
                    }
                }
            };

            await sink.WriteAsync(order);
        }
    }
}
