using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.Onliner.Settings;
using TrackApartmentsApp.Domain.Sinks.Abstract;
using TrackApartmentsApp.Domain.Sinks.Conditions.Interfaces;

namespace TrackApartmentsApp.Domain.Sinks
{
    public sealed class EmailSink : Sink
    {
        private readonly SendGridSettings settings;
        private readonly IEmailCondition condition;
        private readonly ILogger logger;

        public EmailSink(SendGridSettings settings, IEmailCondition condition, ILogger logger)
        {
            this.settings = settings;
            this.condition = condition;
            this.logger = logger;
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
                logger.LogInformation("Email message has been sent.", message);
            }
            else
            {
                logger.LogInformation("Email message has been declined.", message);
            }
        }
    }
}
