using System.Text.RegularExpressions;
using TrackApartmentsApp.Core.Enums;

namespace TrackApartmentsApp.Data.Contracts
{
    public interface IPageRegex
    {
        Regex Expression { get; }

        RegexpContentType Type { get; }
    }
}
