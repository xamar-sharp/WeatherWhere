using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Converters
{
    public class UnixTimeToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long unixTime)
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
                return dateTime.ToString("f", new CultureInfo("ru-RU")).Replace(",","");
            }
            return "НЕКОРРЕКТНАЯ ДАТА";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
