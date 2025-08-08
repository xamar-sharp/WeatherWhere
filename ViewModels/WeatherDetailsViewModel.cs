using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherWhere.Models;
using WeatherWhere.Services;

namespace WeatherWhere.ViewModels
{
    public class WeatherDetailsViewModel : INotifyPropertyChanged
    {
        private NormalizedWeatherData _weatherData;
        public NormalizedWeatherData WeatherData { get => _weatherData; set { _weatherData = value; OnPropertyChanged(); } }
        private string _formattedInfo;
        public string FormattedInfo { get => _formattedInfo; set { _formattedInfo = value; OnPropertyChanged(); } }
        public ICommand LoadFormattedInfo { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public WeatherDetailsViewModel()
        {
            LoadFormattedInfo = new GenericCommand((param) =>
            {
                FormattedInfo = GetDetailedDescription(WeatherData);
            }, (param) => true);
        }
        public void OnPropertyChanged([CallerMemberName] string prop = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void UpdateWeatherData(CurrentWeatherData rawData)
        {
            WeatherData = NormalizedWeatherData.FromCurrentWeather(rawData);
        }
        public void UpdateWeatherData(WeatherListItemFiveDays rawData)
        {
            WeatherData = NormalizedWeatherData.FromForecastItem(rawData, new CityFiveDays() { Name = Application.Current.Resources["CurrentCityNameResource"].ToString()}, "OK");
        }
        public static string GetDetailedDescription(NormalizedWeatherData data)
        {
            if (data == null) return string.Empty;
            var sb = new System.Text.StringBuilder();
            if (data.Latitude != 0 || data.Longitude != 0)
                sb.AppendLine($"Координаты: широта {data.Latitude}°, долгота {data.Longitude}°.");
            if (!string.IsNullOrEmpty(data.CityName) || data.CityId != 0 || !string.IsNullOrEmpty(data.CountryCode) || data.Population != 0)
            {
                var cityLine = "Город: ";
                if (!string.IsNullOrEmpty(data.CityName))
                    cityLine += $"{data.CityName}";
                if (data.CityId != 0)
                    cityLine += $" (ID: {data.CityId})";
                if (!string.IsNullOrEmpty(data.CountryCode))
                    cityLine += $", страна: {data.CountryCode}";
                if (data.Population != 0)
                    cityLine += $", население: {data.Population} человек";
                cityLine += ".";
                sb.AppendLine(cityLine);
            }
            if (data.TimezoneOffsetSeconds != 0)
            {
                var timezone = TimeSpan.FromSeconds(data.TimezoneOffsetSeconds);
                sb.AppendLine($"Смещение часового пояса: {timezone.Hours:+#;-#;0} часов.");
            }
            if (data.TimestampUnix != 0)
            {
                var timezone = TimeSpan.FromSeconds(data.TimezoneOffsetSeconds);
                var dt = DateTimeOffset.FromUnixTimeSeconds(data.TimestampUnix).ToOffset(timezone);
                sb.AppendLine($"Дата и время измерения: {dt:dd.MM.yyyy HH:mm} (локальное время).");
            }
            if (!string.IsNullOrEmpty(data.TimestampText))
                sb.AppendLine($"Текстовое время: {data.TimestampText}.");
            if (data.WeatherConditions != null && data.WeatherConditions.Count > 0)
            {
                sb.AppendLine("Погодные условия:");
                foreach (var cond in data.WeatherConditions)
                {
                    sb.AppendLine($" - {cond.Main} ({cond.Description}), код: {cond.Id}, иконка: {cond.Icon}");
                }
            }
            if (data.Temperature != 0)
                sb.AppendLine($"Температура: {data.Temperature:F1} °C.");
            if (data.FeelsLike != 0)
                sb.AppendLine($"Ощущается как: {data.FeelsLike:F1} °C.");
            if (data.TempMin != 0)
                sb.AppendLine($"Минимальная температура: {data.TempMin:F1} °C.");
            if (data.TempMax != 0)
                sb.AppendLine($"Максимальная температура: {data.TempMax:F1} °C.");
            if (data.Pressure != 0)
                sb.AppendLine($"Давление: {data.Pressure} гПа.");
            if (data.SeaLevelPressure != 0)
                sb.AppendLine($"Давление на уровне моря: {data.SeaLevelPressure} гПа.");
            if (data.GroundLevelPressure != 0)
                sb.AppendLine($"Давление на уровне земли: {data.GroundLevelPressure} гПа.");
            if (data.HumidityPercent != 0)
                sb.AppendLine($"Влажность: {data.HumidityPercent}%.");
            if (data.TemperatureKf != 0)
                sb.AppendLine($"Корректировка температуры: {data.TemperatureKf:F2}.");
            if (data.VisibilityMeters != 0)
                sb.AppendLine($"Видимость: {data.VisibilityMeters} м.");
            if (data.WindSpeed != 0)
                sb.AppendLine($"Скорость ветра: {data.WindSpeed:F1} м/с.");
            if (data.WindDegree != 0)
                sb.AppendLine($"Направление ветра: {data.WindDegree}° = {data.WindDegreeTxt}.");
            if (data.WindGust != 0)
                sb.AppendLine($"Порывы ветра до: {data.WindGust:F1} м/с.");
            if (data.CloudinessPercent != 0)
                sb.AppendLine($"Облачность: {data.CloudinessPercent}%.");
            if (data.RainLastHourMm != 0)
                sb.AppendLine($"Дождь за последний час: {data.RainLastHourMm} мм.");
            if (data.PrecipitationProbability != 0)
                sb.AppendLine($"Вероятность осадков: {data.PrecipitationProbability * 100:F0}%.");
            if (data.SysType != 0 || data.SysId != 0)
            {
                var sysLine = "Системные данные:";
                if (data.SysType != 0)
                    sysLine += $" тип: {data.SysType}";
                if (data.SysId != 0)
                    sysLine += $", ID: {data.SysId}";
                sb.AppendLine(sysLine + ".");
            }
            if (data.SunriseUnix != 0)
            {
                var timezone = TimeSpan.FromSeconds(data.TimezoneOffsetSeconds);
                var sunrise = DateTimeOffset.FromUnixTimeSeconds(data.SunriseUnix).ToOffset(timezone);
                sb.AppendLine($"Восход солнца: {sunrise:HH:mm}.");
            }
            if (data.SunsetUnix != 0)
            {
                var timezone = TimeSpan.FromSeconds(data.TimezoneOffsetSeconds);
                var sunset = DateTimeOffset.FromUnixTimeSeconds(data.SunsetUnix).ToOffset(timezone);
                sb.AppendLine($"Закат солнца: {sunset:HH:mm}.");
            }
            if (!string.IsNullOrEmpty(data.PartOfDay))
                sb.AppendLine($"Часть дня: {data.PartOfDay}.");

            if (!string.IsNullOrEmpty(data.ApiResponseCode))
                sb.AppendLine($"Код ответа API: {data.ApiResponseCode}.");
            return sb.ToString();
        }
    }
}
