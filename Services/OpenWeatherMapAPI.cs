using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using WeatherWhere.Models;
namespace WeatherWhere.Services
{
    public enum WeatherState
    {//Ночь состояние сопровождается фоном, но без изменения погодного аудио соотв. времени
        Dull, Cloudy, Sunny, CloudyWithSun, Rain, RainWithThunder, ThunderWithoutRain, Snowy
    }
    public class OpenWeatherMapAPI
    {
        private const string API_KEY = "b4165464267307e39677757efb016c7b";
        public static WeatherState WeatherState;
        public static bool IsNight;
        private readonly static HttpClient _httpClient;
        /*
           
            BaseAddress =
            new Uri("https://api.openweathermap.org/data/2.5/", UriKind.Absolute)
           */
        private readonly IAudioImageSwitcher _switcher;
        public OpenWeatherMapAPI(IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
        }
        static OpenWeatherMapAPI()
        {
            _httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true
            })
            {
                MaxResponseContentBufferSize = int.MaxValue,
                Timeout = TimeSpan.FromHours(1),
                BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/", UriKind.Absolute)
            };
        }
        public static void SetWeatherStateAndIsNight(CurrentWeatherData data)
        {
            IsNight = DateTime.Now.Hour > 22 || DateTime.Now.Hour < 4;
            if (data.weather.Count == 0)
            {
                WeatherState = WeatherState.Dull;
                return;
            }
            int weatherId = data.weather[0].id;
            if (weatherId == 800)
                WeatherState = WeatherState.Sunny;
            else if (weatherId == 801)
                WeatherState = WeatherState.CloudyWithSun;
            else if (weatherId == 802 || weatherId == 803)
                WeatherState = WeatherState.Cloudy;
            else if (weatherId == 804)
                WeatherState = WeatherState.Dull;
            else if (weatherId >= 200 && weatherId <= 232)
            {
                if (weatherId >= 200 && weatherId <= 202 || weatherId == 230 || weatherId == 231 || weatherId == 232)
                    WeatherState = WeatherState.RainWithThunder;
                else
                    WeatherState = WeatherState.ThunderWithoutRain;
            }
            else if (weatherId >= 300 && weatherId <= 531)
                WeatherState = WeatherState.Rain;
            else if (weatherId >= 600 && weatherId <= 622)
                WeatherState = WeatherState.Snowy;
            else
                WeatherState = WeatherState.Cloudy;
        }
        public static WeatherState GetWeatherStateAndIsNight(int weatherId)
        {
            if (weatherId == 800)
                return WeatherState.Sunny;
            else if (weatherId == 801)
                return WeatherState.CloudyWithSun;
            else if (weatherId == 802 || weatherId == 803)
                return WeatherState.Cloudy;
            else if (weatherId == 804)
                return WeatherState.Dull;
            else if (weatherId >= 200 && weatherId <= 232)
            {
                if (weatherId >= 200 && weatherId <= 202 || weatherId == 230 || weatherId == 231 || weatherId == 232)
                    return WeatherState.RainWithThunder;
                else
                    return WeatherState.ThunderWithoutRain;
            }
            else if (weatherId >= 300 && weatherId <= 531)
                return WeatherState.Rain;
            else if (weatherId >= 600 && weatherId <= 622)
                return WeatherState.Snowy;
            else
                return WeatherState.Cloudy;
        }
        public static bool WasChangedCity = false;
        private bool _firstCache = true;
        //при каждом новом запросе(АУДИО РАЗНОЕ ВЗАВИС. ОТ ВРЕМЕНИ ЗАПРОСА -  СЛИШКОМ СЛОЖНО, АУДИО ОДНО НА ОДНУ СЕССИЮ РАБОТЫ ПРИЛОЖЕНИЯ)
        public async ValueTask<(CurrentWeatherData, bool)> TryGetCurrentWeatherDataFromApiOrCache()
        {
            if (Preferences.ContainsKey("CurrentWeatherData") && Preferences.ContainsKey("CurrentWeatherRequestDate") && !WasChangedCity)
            {
                var lastRequestDate = Preferences.Get("CurrentWeatherRequestDate", DateTime.MaxValue);
                if (!(DateTime.Now > lastRequestDate.AddMinutes(5)))//10 minutes for CurrentRequest
                {
                    string json = Preferences.Get("CurrentWeatherData", null);
                    CurrentWeatherData res = System.Text.Json.JsonSerializer.Deserialize<CurrentWeatherData>(json);
                    SetWeatherStateAndIsNight(res);
                    if (_firstCache)
                    {
                        _switcher.SetBackgroundAudio();
                        _firstCache = false;
                    }
                    return (res, true);
                }
            }
            var weather = await TryGetCurrentWeather(BuildParameters(API_KEY, countryName: Application.Current.Resources["CurrentCityNameResource"].ToString()
                .Replace("-", "%20")));
            if (weather.Item2)
            {
                SetWeatherStateAndIsNight(weather.Item1);
                _switcher.SetBackgroundAudio();
                if (WasChangedCity)
                {
                    WasChangedCity = false;
                }
                Preferences.Set("CurrentWeatherData", System.Text.Json.JsonSerializer.Serialize(weather.Item1));
                Preferences.Set("CurrentWeatherRequestDate", DateTime.Now);
                return (weather.Item1, true);
            }
            else
            {
                return (null, false);
            }
        }
        public async ValueTask<(WeatherDataFiveDays, bool)> TryGetFiveDaysWeatherDataFromApiOrCache()
        {
            if (Preferences.ContainsKey("FiveDaysWeatherData") && Preferences.ContainsKey("FiveDaysWeatherRequestDate") && !WasChangedCity)
            {
                var lastRequestDate = Preferences.Get("FiveDaysWeatherRequestDate", DateTime.MaxValue);
                if (!(DateTime.Now > lastRequestDate.AddHours(1)))
                {
                    string json = Preferences.Get("FiveDaysWeatherData", null);
                    WeatherDataFiveDays res = System.Text.Json.JsonSerializer.Deserialize<WeatherDataFiveDays>(json);
                    return (res, true);
                }
            }
            var parameters = BuildParameters(API_KEY, countryName: Application.Current.Resources["CurrentCityNameResource"].ToString().Replace("-","%20"), timeRowsCount:40);
            var weather = await TryGetThreeHoursFiveDays(parameters);
            if (WasChangedCity)
            {
                WasChangedCity = false;
            }
            if (weather.Item2)
            {
                Preferences.Set("FiveDaysWeatherData", System.Text.Json.JsonSerializer.Serialize(weather.Item1));
                Preferences.Set("FiveDaysWeatherRequestDate", DateTime.Now);
                return (weather.Item1, true);
            }
            else
            {
                return (null, false);
            }
        }
        public async ValueTask<(WeatherDataFiveDays, bool)> TryGetThreeHoursFiveDays(OpenWeatherMapAPIRequestParameters parameters)
        {
            (string, bool) creatingResult = CreateRequestString("forecast", parameters);
            if (!creatingResult.Item2)
            {
                return (null, false);
            }
            var response = await _httpClient.GetAsync(creatingResult.Item1);
            if (!response.IsSuccessStatusCode)
            {
                return (null, false);
            }
            var json = await response.Content.ReadAsStringAsync();
            WeatherDataFiveDays? obj = JsonConvert.DeserializeObject<WeatherDataFiveDays>(json);
            return (obj, obj is not null);
        }
        public async ValueTask<(CurrentWeatherData?, bool)> TryGetCurrentWeather(OpenWeatherMapAPIRequestParameters parameters)
        {
            (string, bool) creatingResult = CreateRequestString("weather", parameters);
            if (!creatingResult.Item2)
            {
                Debug.WriteLine(creatingResult.Item1);
                return (null, false);
            }
            var response = await _httpClient.GetAsync(creatingResult.Item1);
            if (!response.IsSuccessStatusCode)
            {
                return (null, false);
            }
            var json = await response.Content.ReadAsStringAsync();
            CurrentWeatherData? obj = JsonConvert.DeserializeObject<CurrentWeatherData>(json);
            return (obj, obj is not null);
        }
        public static OpenWeatherMapAPIRequestParameters BuildParameters(string apiKey, ResponseMode responseFormat = ResponseMode.Json, string countryName = "Moscow", int? timeRowsCount = 1,
            double? latitude = null, double? longitude = null, ResponseLanguage languageOfResponse = ResponseLanguage.ru, ResponseLanguage countryCode = ResponseLanguage.ru,
            int? zipCode = null, int? cityId = null, ResponseUnit unitsOfDimension = ResponseUnit.Metric) =>
            new OpenWeatherMapAPIRequestParameters()
            {
                AppId = apiKey,
                Mode = responseFormat,
                Q = countryName,
                Cnt = timeRowsCount,
                Lat = latitude,
                Lon = longitude,
                Lang = languageOfResponse,
                CountryCode = countryCode,
                ZipCode = zipCode,
                Id = cityId
            };
        public static (string, bool) CreateRequestString(string specificApiRawMark, OpenWeatherMapAPIRequestParameters parameters)
        {
            StringBuilder builder = new StringBuilder(256);
            builder.Append(specificApiRawMark + "?");
            if (parameters.Mode != ResponseMode.Json)
            {
                builder.Append("mode=" + parameters.Mode.ToString().ToLower() + "&");
            }
            builder.Append("lang=" + parameters.Lang.ToString().ToLower() + "&");
            if (parameters.Lon.HasValue && parameters.Lat.HasValue)
            {
                builder.Append("lon=" + parameters.Lon + "&lat=" + parameters.Lat);
            }
            else if (parameters.ZipCode.HasValue)
            {
                builder.Append("zip=" + parameters.ZipCode + "," + parameters.CountryCode.ToString().ToLower());
            }
            else if (parameters.Id.HasValue)
            {
                builder.Append("id=" + parameters.Id);
            }
            else if (!string.IsNullOrEmpty(parameters.Q))
            {
                builder.Append("q=" + parameters.Q);
            }
            else
            {
                return ("Source Not Defined Error", false);
            }
            builder.Append("&");
            if (parameters.Cnt.HasValue)
            {
                builder.Append("cnt=" + parameters.Cnt + "&");
            }
            if (!string.IsNullOrEmpty(parameters.AppId))
            {
                builder.Append("appid=" + parameters.AppId + "&units=" + parameters.Units.ToString().ToLower());
            }
            else
            {
                return ("Api Key Not Defined Error", false);
            }
            return (builder.ToString(), true);
        }
    }
}
