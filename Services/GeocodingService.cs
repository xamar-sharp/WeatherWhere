using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Services
{
    public interface IGeocodingService
    {
        ValueTask<string> GetPlacemarksInfo(double longitude, double latitude);
    }
    public class GeocodingService : IGeocodingService
    {
        public async ValueTask<string> GetPlacemarksInfo(double latitude, double longitude)
        {
            try
            {
                var placemarksRaw = await Geocoding.GetPlacemarksAsync(longitude, latitude);
                var placemarks = new List<Placemark>();
                foreach (var item in placemarksRaw)
                {
                    placemarks.Add(item);
                }
                if (placemarks.Count == 0)
                    return "Местоположение не найдено.";
                StringBuilder builder = new StringBuilder(64);
                foreach (Placemark placemark in placemarks)
                {
                    builder.AppendLine("Название: " + placemark.FeatureName + "\n" +
                                       "Страна: " + placemark.CountryName + "\n" +
                                       "Город: " + placemark.Locality + "\n" +
                                       "Улица: " + placemark.Thoroughfare + "\n" +
                                       "Расположение: " + placemark.Location);
                    builder.AppendLine("***********************");
                }
                return builder.ToString();
            }
            catch (ObjectDisposedException ode)
            {
                return $"Ошибка доступа к геоданным (объект уничтожен): {ode.Message}";
            }
            catch (Exception ex)
            {
                return $"Не удалось получить адрес: {ex.Message}";
            }
        }
    }
}
