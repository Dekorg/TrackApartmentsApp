using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrackApartments.Contracts.PageParser;
using TrackApartments.Contracts.PageParser.Abstract;

namespace TrackApartments.Onliner.Domain.PageParsers.Onliner
{
    public class OnlinerPageParser : PageParser, IOnlinerPageParser
    {
        public override IEnumerable<string> FindByRegex(string content, Regex regex)
        {
            var parsed = base.FindByRegex(content, regex).ToList();

            var results = new List<string>();

            foreach (var item in parsed)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    var result = item
                        .Replace(" ", "")
                        .Replace("-", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace(";", "");

                    result = result.Trim();

                    if (!result.StartsWith("+"))
                    {
                        result = "+" + result;
                    }

                    results.Add(result);
                }
            }

            return new HashSet<string>(results).ToList();
        }
    }
}
