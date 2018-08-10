using System;
using System.Collections.Generic;
using System.Text;
using TrackApartments.Contracts;

namespace TrackApartments.User.Domain.Queue.Contracts
{
    public interface IEmailQueueWriter<T> : IQueueWriter<T>
    {
    }
}
