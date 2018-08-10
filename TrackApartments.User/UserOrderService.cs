

using System.Threading.Tasks;
using TrackApartments.Contracts.Models;
using TrackApartmentsApp.Domain.Sinks.Abstract;

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
                Apartment = apartment
            };

            order.UserInfo = new UserInfo();
            order.UserInfo.UserName = "Vitaly Bibikov";
            order.UserInfo.Phone = "+375291602219";
            order.UserInfo.Email = "evilavenger@yandex.ru";

            order.SinkSettings = new UserSinkSettings();

            order.SinkSettings.EmailSettings = new SinkSettings
            {
                DesiredPriceBorder = 400,
                IsNewPeriod = 1,
                IsOnlyOwner = false
            };

            order.SinkSettings.SmsSettings = new SinkSettings
            {
                DesiredPriceBorder = 300,
                IsNewPeriod = 1,
                IsOnlyOwner = true
            };

            await sink.WriteAsync(order);
        }
    }
}
