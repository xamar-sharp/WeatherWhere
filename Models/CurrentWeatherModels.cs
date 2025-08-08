using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WeatherWhere.Models
{
    public class CurrentWeatherData
    {
        public Coord coord { get; set; }         // Географические координаты (долгота/широта)
        public List<Weather> weather { get; set; } // Список погодных условий
        [JsonProperty("base")]
        public string @base { get; set; }         // Источник данных (обычно "stations")
        public MainData main { get; set; }           // Основные метеорологические данные
        public int visibility { get; set; }       // Видимость в метрах (макс. 10000)
        public Wind wind { get; set; }           // Данные о ветре
        public Rain rain { get; set; }           // Количество осадков (может быть null)
        public Clouds clouds { get; set; }       // Облачность
        public long dt { get; set; }              // Время расчета данных (Unix timestamp)
        public Sys sys { get; set; }             // Системная информация
        public int timezone { get; set; }         // Смещение часового пояса в секундах
        public int id { get; set; }               // ID города
        public string name { get; set; }          // Название местности
        public int cod { get; set; }              // Код ответа API (200 = успешно)
    }

    public class Coord
    {
        public double lon { get; set; }  // Долгота (от -180 до 180)
        public double lat { get; set; }  // Широта (от -90 до 90)
    }

    public class Weather
    {
        public int id { get; set; }           // Код погодного условия
        public string main { get; set; }      // Группа погодных параметров (Rain, Snow и т.д.)
        public string description { get; set; } // Текстовое описание погоды
        public string icon { get; set; }      // Идентификатор иконки погоды
    }

    public class MainData
    {
        public double temp { get; set; }      // Температура (Кельвины)
        public double feels_like { get; set; } // Ощущаемая температура (Кельвины)
        public double temp_min { get; set; }  // Минимальная температура (Кельвины)
        public double temp_max { get; set; }  // Максимальная температура (Кельвины)
        public int pressure { get; set; }     // Атмосферное давление (гПа)
        public int humidity { get; set; }     // Влажность (%)
        public int sea_level { get; set; }    // Давление на уровне моря (гПа)
        public int grnd_level { get; set; }   // Давление на уровне земли (гПа)
    }

    public class Wind
    {
        public double speed { get; set; } // Скорость ветра (м/с)
        public int deg { get; set; }      // Направление ветра (градусы)
        public double gust { get; set; }  // Порывы ветра (м/с)
    }

    public class Rain
    {
        public double _1h { get; set; } // Количество осадков за последний час (мм)
    }

    public class Clouds
    {
        public int all { get; set; } // Процент облачности (%)
    }

    public class Sys
    {
        public int type { get; set; }    // Тип системной информации
        public int id { get; set; }      // ID системы
        public string country { get; set; } // Код страны (2 символа)
        public long sunrise { get; set; } // Время восхода (Unix timestamp)
        public long sunset { get; set; }  // Время заката (Unix timestamp)
    }
}
