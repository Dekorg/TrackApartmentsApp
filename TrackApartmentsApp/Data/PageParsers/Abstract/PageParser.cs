using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrackApartmentsApp.Core.Interfaces;

namespace TrackApartmentsApp.Data.PageParsers.Abstract
{
    public abstract class PageParser : IPageParser
    {
        public virtual IEnumerable<string> FindByRegex(string content, Regex regex)
        {
            var results = regex.Matches(content);

            foreach (Match item in results)
            {
                yield return item.Value;
            }
        }
    }
}
