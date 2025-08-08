using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Models;
using WeatherWhere.Services;
namespace WeatherWhere.Converters
{
    public class WeatherToIconConverter : IValueConverter
    {
        public object Convert(object? obj, Type type, object? param, CultureInfo culture)
        {
            return AssetManager.GetWeatherIconPath(OpenWeatherMapAPI.GetWeatherStateAndIsNight((obj as List<WeatherFiveDays>)[0].Id));
        }
        public object ConvertBack(object? obj, Type type, object? param, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
