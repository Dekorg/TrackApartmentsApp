using System.Text.RegularExpressions;
using TrackApartmentsApp.Core.Enums;

namespace TrackApartmentsApp.Data.Regexps
{
    public interface IPageRegex
    {
        Regex Expression { get; }

        RegexpContentType Type { get; }
    }
}
