using System;
using TrackApartments.Contracts;

namespace TrackApartments.User.Domain.Queue.Contracts
{
    public interface ISmsQueueWriter<T> : IQueueWriter<T>
    {
    }
}
