using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Models
{
    public class NormalizedWeatherData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public int Population { get; set; }
        public int TimezoneOffsetSeconds { get; set; }
        public long TimestampUnix { get; set; }
        public string TimestampText { get; set; }
        public List<WeatherCondition> WeatherConditions { get; set; } = new();
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Pressure { get; set; }
        public int SeaLevelPressure { get; set; }
        public int GroundLevelPressure { get; set; }
        public int HumidityPercent { get; set; }
        public double TemperatureKf { get; set; }
        public int VisibilityMeters { get; set; }
        public double WindSpeed { get; set; }
        public int WindDegree { get; set; }
        public string WindDegreeTxt { get => DegreesToRussianCompass8((double)WindDegree); set { } }
        public double WindGust { get; set; }
        public int CloudinessPercent { get; set; }
        public double RainLastHourMm { get; set; }
        public double PrecipitationProbability { get; set; }
        public int SysType { get; set; }
        public int SysId { get; set; }
        public long SunriseUnix { get; set; }
        public long SunsetUnix { get; set; }
        public string PartOfDay { get; set; }
        public string ApiResponseCode { get; set; }
        private static readonly string[] Directions8 = { "С", "СВ", "В", "ЮВ", "Ю", "ЮЗ", "З", "СЗ" };
        public static string DegreesToRussianCompass8(double degrees)
        {
            degrees = (degrees % 360 + 360) % 360;
            int index = (int)Math.Round(degrees / 45) % 8;
            return Directions8[index];
        }
        public static NormalizedWeatherData FromCurrentWeather(CurrentWeatherData current)
        {
            return new NormalizedWeatherData
            {
                Latitude = current.coord?.lat ?? 0,
                Longitude = current.coord?.lon ?? 0,
                CityId = current.id,
                CityName = current.name,
                CountryCode = current.sys?.country,
                TimezoneOffsetSeconds = current.timezone,
                SunriseUnix = current.sys?.sunrise ?? 0,
                SunsetUnix = current.sys?.sunset ?? 0,
                TimestampUnix = current.dt,
                ApiResponseCode = current.cod.ToString(),
                WeatherConditions = current.weather?.Select(w => new WeatherCondition
                {
                    Id = w.id,
                    Main = w.main,
                    Description = w.description,
                    Icon = w.icon
                }).ToList(),
                Temperature = current.main?.temp ?? 0,
                FeelsLike = current.main?.feels_like ?? 0,
                TempMin = current.main?.temp_min ?? 0,
                TempMax = current.main?.temp_max ?? 0,
                Pressure = current.main?.pressure ?? 0,
                SeaLevelPressure = current.main?.sea_level ?? 0,
                GroundLevelPressure = current.main?.grnd_level ?? 0,
                HumidityPercent = current.main?.humidity ?? 0,
                VisibilityMeters = current.visibility,
                WindSpeed = current.wind?.speed ?? 0,
                WindDegree = current.wind?.deg ?? 0,
                WindGust = current.wind?.gust ?? 0,
                CloudinessPercent = current.clouds?.all ?? 0,
                RainLastHourMm = current.rain?._1h ?? 0,
                SysType = current.sys?.type ?? 0,
                SysId = current.sys?.id ?? 0
            };
        }
        //преобразование одной временной строчки данных о погоде в модель нормализованной информации в деталях о погоде на этом отрезке
        public static NormalizedWeatherData FromForecastItem(WeatherListItemFiveDays item, CityFiveDays city, string cod)//последние два свойства передаются от WeatherData
        {
            return new NormalizedWeatherData
            {
                Latitude = city?.Coord?.Lat ?? 0,
                Longitude = city?.Coord?.Lon ?? 0,
                CityId = city?.Id ?? 0,
                CityName = city?.Name,
                CountryCode = city?.Country,
                Population = city?.Population ?? 0,
                TimezoneOffsetSeconds = city?.Timezone ?? 0,
                SunriseUnix = city?.Sunrise ?? 0,
                SunsetUnix = city?.Sunset ?? 0,
                TimestampUnix = item.Dt,
                TimestampText = item.DtTxt,
                ApiResponseCode = cod,
                WeatherConditions = item.Weather?.Select(w => new WeatherCondition
                {
                    Id = w.Id,
                    Main = w.Main,
                    Description = w.Description,
                    Icon = w.Icon
                }).ToList(),
                Temperature = item.Main?.Temp ?? 0,
                FeelsLike = item.Main?.FeelsLike ?? 0,
                TempMin = item.Main?.TempMin ?? 0,
                TempMax = item.Main?.TempMax ?? 0,
                Pressure = item.Main?.Pressure ?? 0,
                SeaLevelPressure = item.Main?.SeaLevel ?? 0,
                GroundLevelPressure = item.Main?.GrndLevel ?? 0,
                HumidityPercent = item.Main?.Humidity ?? 0,
                TemperatureKf = item.Main?.TempKf ?? 0,
                VisibilityMeters = item.Visibility,
                PrecipitationProbability = item.Pop,
                WindSpeed = item.Wind?.Speed ?? 0,
                WindDegree = item.Wind?.Deg ?? 0,
                WindGust = item.Wind?.Gust ?? 0,
                CloudinessPercent = item.Clouds?.All ?? 0,
                PartOfDay = item.Sys?.Pod
            };
        }
    }
    public class WeatherCondition
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
