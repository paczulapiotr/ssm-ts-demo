using System;
using System.Globalization;

namespace DemoAPI.Common
{
    public static class DateParserHelper
    {
        public static DateTime Parse(string date) =>
            DateTime.TryParseExact(date, Configuration.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var @out)
                ? @out
                : DateTime.Today;
    }
}
