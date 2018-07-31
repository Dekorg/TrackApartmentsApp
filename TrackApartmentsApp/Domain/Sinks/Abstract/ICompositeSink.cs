using System;
using System.Collections.Generic;
using System.Text;

namespace TrackApartmentsApp.Domain.Sinks.Abstract
{
    public interface ICompositeSink<T> : ISink<T>
    {
    }
}
