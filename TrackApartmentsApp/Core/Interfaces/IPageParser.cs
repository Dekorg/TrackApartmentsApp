using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TrackApartmentsApp.Core.Interfaces
{
    public interface IPageParser
    {
        IEnumerable<string> FindByRegex(string content, Regex regex);
    }
}
