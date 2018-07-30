using System.Text.RegularExpressions;
using TrackApartmentsApp.Core.Enums;

namespace TrackApartmentsApp.Data.Regexps
{
    public class PhoneRegex : IPageRegex
    {
        public Regex Expression =>
            new Regex(
                "[+]*(\\s*)375([(\\]]|\\s|-)*(24|25|29|33|44)+(\\s|[)\\]])*(\\s|-)*(\\d[\\s-]*){3,22}\\d",
                RegexOptions.Compiled);

        public RegexpContentType Type => RegexpContentType.Phone;
    }
}
