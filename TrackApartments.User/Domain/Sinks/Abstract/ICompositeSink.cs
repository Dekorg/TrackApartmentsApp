using System;
using System.Collections.Generic;
using System.Text;
using TrackApartments.User.Domain.Sinks.Abstract;

namespace TrackApartmentsApp.Domain.Sinks.Abstract
{
    public interface ICompositeSink<T> : ISink<T>
    {
    }
}
