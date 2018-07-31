using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Sinks.Abstract;
using TrackApartmentsApp.Domain.Sinks.Conditions;

namespace TrackApartmentsApp.Domain.Sinks
{
    public class EmailSink : Sink
    {
        private readonly SendGridSettings settings;
        private readonly IEmailCondition condition;

        public EmailSink(SendGridSettings settings, IEmailCondition condition)
        {
            this.settings = settings;
            this.condition = condition;
        }

        public override async Task WriteAsync(Apartment message)
        {
            if (condition.IsValid(message))
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(settings.From),
                    Subject = "New Apartment on " + message.Address,
                    Body = message.ToString(),
                    To = { settings.To }
                };

                var smtpClient = new SmtpClient
                {
                    Credentials = new NetworkCredential(settings.UserName, settings.Password),
                    Host = settings.Host,
                    Port = settings.Port
                };

                await smtpClient.SendMailAsync(mailMessage);
            }

        }
    }
}
