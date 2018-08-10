using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrackApartments.Contracts.Models;
using TrackApartments.User.Domain.Queue.Contracts;
using TrackApartments.User.Domain.Sinks.Abstract;
using TrackApartments.User.Domain.Sinks.Conditions.Interfaces;

namespace TrackApartments.User.Domain.Sinks
{
    public sealed class SmsSink : Sink
    {
        private readonly ISmsQueueWriter<Order> writer;
        private readonly ISmsCondition condition;
        private readonly ILogger logger;

        public SmsSink(ISmsQueueWriter<Order> writer, ISmsCondition condition, ILogger logger)
        {
            this.writer = writer;
            this.condition = condition;
            this.logger = logger;
        }

        public override async Task WriteAsync(Order item)
        {
            if (condition.IsValid(item))
            {
                await writer.WriteAsync(item);
                logger.LogDebug("Sms message has added to queue.", item);
            }
            else
            {
                logger.LogDebug("Sms message has been declined.", item, condition);
            }
        }
    }
}
