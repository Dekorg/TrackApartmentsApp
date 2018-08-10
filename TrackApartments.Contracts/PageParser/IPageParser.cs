using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TrackApartments.Contracts.PageParser
{
    public interface IPageParser
    {
        IEnumerable<string> FindByRegex(string content, Regex regex);
    }
}
