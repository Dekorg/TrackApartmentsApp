using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TrackApartments.Contracts.PageParser.Abstract
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
