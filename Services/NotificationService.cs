using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Models;
namespace WeatherWhere.Services
{
    public interface INotificationService
    {
        bool DisableNotifications();
        ValueTask<bool> EnableNotifications();
    }
    public class NotificationService : INotificationService
    {
        private readonly OpenWeatherMapAPI _weatherApi;
        public NotificationService(OpenWeatherMapAPI weatherApi)
        {
            _weatherApi = weatherApi;
        }
        public bool DisableNotifications()
        {
            return LocalNotificationCenter.Current.CancelAll();
        }
        public async ValueTask<bool> EnableNotifications()
        {
            (CurrentWeatherData?, bool) data = await _weatherApi.TryGetCurrentWeatherDataFromApiOrCache();
            string description = "В кэше данных о текущей погоде нет, интернета нет";
            if (data.Item2)
            {
                description = GenerateWeatherNotification(data.Item1);
            }
            var notification = new NotificationRequest
            {
                NotificationId = 1,
                Title = "WeatherWhere",
                Description = description,
                Android = { IconLargeName = new Plugin.LocalNotification.AndroidOption.AndroidIcon("notificationsicon") },
                Schedule =
                    {
                        NotifyTime = DateTime.Now.AddMinutes(1),
                        RepeatType = NotificationRepeat.TimeInterval,
                        NotifyRepeatInterval = TimeSpan.FromHours(12)
                    }
            };
            return await LocalNotificationCenter.Current.Show(notification);
        }
        public string GenerateWeatherNotification(CurrentWeatherData data)
        {
            double tempC = data.main.temp;
            double feelsLikeC = data.main.feels_like;
            double tempMinC = data.main.temp_min;
            double tempMaxC = data.main.temp_max;
            string weatherDescription = data.weather?.FirstOrDefault()?.description ?? "нет данных";
            DateTime sunrise = DateTimeOffset.FromUnixTimeSeconds(data.sys.sunrise).LocalDateTime;
            DateTime sunset = DateTimeOffset.FromUnixTimeSeconds(data.sys.sunset).LocalDateTime;
            string notification = $"""
            🌍 {data.name}, {data.sys.country}
            ⛅ Погода: {weatherDescription}
            🌡️ Температура: {tempC:0.#}°C (ощущается как {feelsLikeC:0.#}°C)
            📈 Мин/макс: {tempMinC:0.#}°C / {tempMaxC:0.#}°C
            💧 Влажность: {data.main.humidity}%
            🌬️ Ветер: {data.wind.speed} м/с (направление {data.wind.deg}°)
            ☁️ Облачность: {data.clouds.all}%
            🎯 Давление: {data.main.pressure} гПа
            🌅 Восход: {sunrise:HH:mm}
            🌇 Закат: {sunset:HH:mm}
            """;
            return notification;
        }
    }
}
