using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;

namespace WeatherWhere.Converters
{
    public class ImageStreamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageName = parameter as string;
            if (string.IsNullOrEmpty(imageName))
                return null;
            return new StreamImageSource
            {
                Stream = async ct =>
                {
                    var stream = await FileSystem.OpenAppPackageFileAsync($"Resources/Images/{imageName}");
                    return stream;
                }
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
