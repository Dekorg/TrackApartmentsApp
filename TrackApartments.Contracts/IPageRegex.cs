using System.Text.RegularExpressions;
using TrackApartments.Shared.Enums;

namespace TrackApartments.Contracts
{
    public interface IPageRegex
    {
        Regex Expression { get; }

        RegexpContentType Type { get; }
    }
}
