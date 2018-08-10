using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Queue.Contracts;
using TrackApartments.User.Domain.Sinks.Abstract;
using TrackApartments.User.Domain.Sinks.Conditions.Interfaces;

namespace TrackApartments.User.Domain.Sinks
{
    public sealed class EmailSink : Sink
    {
        private readonly IEmailQueueWriter<Order> writer;
        private readonly IEmailCondition condition;
        private readonly ILogger logger;

        public EmailSink(IEmailQueueWriter<Order> writer, IEmailCondition condition, ILogger logger)
        {
            this.writer = writer;
            this.condition = condition;
            this.logger = logger;
        }

        public override async Task WriteAsync(Order message)
        {
            if (condition.IsValid(message))
            {
                await writer.WriteAsync(message);
                logger.LogDebug("Email message has been added to queue.", message);
            }
            else
            {
                logger.LogDebug("Email message has been declined.", message, condition);
            }
        }
    }
}
