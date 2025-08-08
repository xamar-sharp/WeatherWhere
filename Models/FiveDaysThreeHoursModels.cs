using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Models
{
    public class WeatherDataFiveDays
    {
        public string Cod { get; set; }          // Код ответа API (например "200" - успешный запрос)
        public int Message { get; set; }         // Дополнительное сообщение от API (обычно 0)
        public int Cnt { get; set; }             // Количество временных интервалов в прогнозе
        public List<WeatherListItemFiveDays> List { get; set; } // Список прогнозов по временным интервалам
        public CityFiveDays City { get; set; }          // Информация о городе/местности
    }

    public class WeatherListItemFiveDays
    {
        public long Dt { get; set; }             // Временная метка прогноза (Unix timestamp)
        public MainDataFiveDays Main { get; set; }      // Основные метеорологические данные
        public List<WeatherFiveDays> Weather { get; set; } // Описание погодных условий
        public CloudsFiveDays Clouds { get; set; }      // Данные об облачности
        public WindFiveDays Wind { get; set; }          // Данные о ветре
        public int Visibility { get; set; }      // Видимость в метрах (макс. 10000 = 10км)
        public double Pop { get; set; }          // Вероятность осадков (0-1)
        public SysFiveDays Sys { get; set; }           // Системная информация (часть суток)
        public string DtTxt { get; set; }       // Время прогноза в формате строки
    }

    public class MainDataFiveDays
    {
        public double Temp { get; set; }         // Температура (в Кельвинах)
        public double FeelsLike { get; set; }    // Ощущаемая температура (Кельвины)
        public double TempMin { get; set; }      // Минимальная температура (Кельвины)
        public double TempMax { get; set; }      // Максимальная температура (Кельвины)
        public int Pressure { get; set; }        // Атмосферное давление (гПа)
        public int SeaLevel { get; set; }        // Давление на уровне моря (гПа)
        public int GrndLevel { get; set; }       // Давление на уровне земли (гПа)
        public int Humidity { get; set; }        // Влажность (%)
        public double TempKf { get; set; }       // Коррекция температуры (для внутренних расчетов)
    }

    public class WeatherFiveDays
    {
        public int Id { get; set; }              // Код погодного условия
        public string Main { get; set; }         // Основная группа погоды (Rain, Snow и т.д.)
        public string Description { get; set; }  // Детальное описание погоды
        public string Icon { get; set; }         // Код иконки погоды (например "10d")
    }

    public class CloudsFiveDays
    {
        public int All { get; set; }             // Процент облачности (%)
    }

    public class WindFiveDays
    {
        public double Speed { get; set; }        // Скорость ветра (м/с)
        public int Deg { get; set; }             // Направление ветра (градусы, 0-360)
        public double Gust { get; set; }         // Порывы ветра (м/с)
    }

    public class SysFiveDays
    {
        public string Pod { get; set; }          // Часть суток ("d" - день, "n" - ночь)
    }

    public class CityFiveDays
    {
        public int Id { get; set; }              // ID города в OpenWeatherMap
        public string Name { get; set; }         // Название города/местности
        public CoordFiveDays Coord { get; set; }        // Географические координаты
        public string Country { get; set; }      // Код страны (2 символа)
        public int Population { get; set; }      // Население города
        public int Timezone { get; set; }        // Смещение часового пояса (секунды)
        public long Sunrise { get; set; }        // Время восхода (Unix timestamp)
        public long Sunset { get; set; }         // Время заката (Unix timestamp)
    }

    public class CoordFiveDays
    {
        public double Lat { get; set; }          // Широта (-90 до 90 градусов)
        public double Lon { get; set; }          // Долгота (-180 до 180 градусов)
    }
}
