using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Services
{
    public static class UnixTimestampToDateTimeConverter
    {
        public static DateTime ConvertToUtc(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
        }
        public static DateTime ConvertToLocal(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).LocalDateTime;
        }
        public static DateTime ConvertToGlobal(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
        }
    }
}
