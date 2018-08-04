using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using TrackApartments.Contracts.Models;

namespace TrackApartmentsApp
{
    public static class TrackApartmentsSendGridFunction
    {
        [FunctionName("TrackApartmentsSendGridFunction")]
        [return: SendGrid(ApiKey = "SendGridAPIKey", From = "donotreply@trackapartments.com")]
        public static SendGridMessage Run([QueueTrigger("newordersemailqueue", Connection = "QueueConnectionString")]  Order order, ILogger log)
        {
            log.LogDebug($"{nameof(TrackApartmentsSendGridFunction)} has started.", order);

            var message = new SendGridMessage
            {
                Subject = "New Apartment on \n" + order.Apartment.Address,
            };

            message.AddContent("text/plain", order.Apartment.ToString());
            message.AddTo(new EmailAddress(order.UserInfo.Email, order.UserInfo.UserName));

            log.LogDebug($"New email message has been sent to: {order.UserInfo.Email}", message, order);

            return message;
        }
    }
}
