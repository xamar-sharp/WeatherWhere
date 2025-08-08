using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Services
{
    public enum Audios
    {
        Cloudy, CloudyWithSun, Dull, Rain, RainWithThunder, Snowy, Sunny, ThunderWithoutRain, Windy
    }
    public enum Images
    {
        AboutCity, AboutUs, CloudnessPercents, Cloudy, CloudyBack, CloudyWithSunBack, CloudyWithSun,
        DisableNotifications, EnableNotifications, DubaiCity, DullBack, Humidity, KamchatkaCity, LondonCity, MoscowCity,
        MuteAudio, NewYorkCity, NightBack, Night, NotificationIcon, ObninskCity, ParisCity, PekinCity, Pressure,
        Rain, RainBack, RioCity, RostovOnDonCity, Search, Settings, ShanghaiCity, Snowy, SnowyBack, Sunny, SunnyBack, SydneyCity,
        Thermometer, TokyoCity, TorontoCity, Volume, WeatherMainTab, Weather, Wind, RainWithThunder, DullIcon, RainWithThunderIcon, RainWithSunIcon
    }
    public interface IAssetManager
    {
        ValueTask<Stream> GetImageStream(Images intent);
        ValueTask<Stream> GetWeatherBackgroundStream(WeatherState state);
        ValueTask<Stream> GetCityBackgroundStream(string cityName);
        ValueTask<Stream> GetAudioWeatherStream(WeatherState state);
        ValueTask<Stream> GetWeatherIconStream(WeatherState state);
    }
    public class AssetManager : IAssetManager
    {
        private readonly Dictionary<WeatherState, string> _weatherStateToPathAudioMapper =
        new Dictionary<WeatherState, string>(9)//CurrentImageIntent, CurrentAudioIntent хранить как/брать из кэша и искать и CurrentWeatherState в АПИ Погоды
        {
            {WeatherState.Cloudy,"cloudy.mp3" },
            {WeatherState.CloudyWithSun,"cloudywithsun.mp3" },
            {WeatherState.Dull,"dull.mp3" },
            {WeatherState.Rain,"rain.mp3" },
            {WeatherState.RainWithThunder,"rainwiththunder.mp3" },
            {WeatherState.Snowy,"snowy.mp3" },
            {WeatherState.Sunny,"sunny.mp3" },
            {WeatherState.ThunderWithoutRain,"thunderwithoutrain.mp3" }
        };
        private readonly Dictionary<Images, string> _intentToPathImagesMapper =
        new Dictionary<Images, string>(45)
        {
            { Images.AboutCity,"aboutcity.png" }, {Images.AboutUs,"aboutus.png" }, {Images.CloudnessPercents,"cloudnesspercents.png" } ,{Images.Cloudy,"cloudy.png" },
            { Images.CloudyBack,"cloudy1.jpg" },
            { Images.CloudyWithSunBack,"cloudywithsun.jpg" },{Images.CloudyWithSun,"cloudywithsun.png" },
            { Images.DisableNotifications,"disablenotifications.png" },{ Images.EnableNotifications,"enablenotifications.png" },
            { Images.DubaiCity,"dubai.jpg" }, {Images.DullBack,"dull.jpg" },
            { Images.Humidity,"humiditytab.png" }, {Images.KamchatkaCity,"kamchatka.jpg" },
            { Images.LondonCity,"london.jpg" },{ Images.MoscowCity,"moscow.jpg" },
            { Images.MuteAudio,"mute.png" },{ Images.NewYorkCity,"newyork.jpg" },
            { Images.NightBack,"night1.jpg" },{ Images.Night,"night.png" },{ Images.NotificationIcon,"notificationsicon.png" },
            { Images.ObninskCity,"obninsk,jpg" },{ Images.ParisCity,"paris.jpg" },
            { Images.PekinCity,"pekin.jpg" },  {Images.Pressure,"pressure.png" },
            { Images.Rain,"rain.png" },{ Images.RainBack,"rain1.jpg" },
            { Images.RioCity,"rio.jpg" }, {Images.RostovOnDonCity,"rostovondon.jpg" },{ Images.Search,"search.png" },
            { Images.Settings,"settings.png" },{Images.ShanghaiCity,"shanghai.jpg" },{Images.Snowy,"snowy.png" },
            { Images.SnowyBack,"snowy1.jpg" },{ Images.Sunny,"sunny.png" },{ Images.SunnyBack,"sunny1.jpg" },{ Images.SydneyCity,"sydney.jpg" },
            { Images.Thermometer,"thermometer.png" }, {Images.TokyoCity,"tokyo.jpg" }, {Images.TorontoCity,"toronto.jpg" }, {Images.Volume,"volume.png" },
            { Images.WeatherMainTab,"weathermaintab.png" } ,{ Images.Weather,"weathertab.png" }, {Images.Wind,"wind.png" },{Images.RainWithThunder,"rainwiththunder.jpg"},
            {Images.DullIcon,"dull1.png" },{Images.RainWithThunderIcon,"rainwiththunder1.png"},{Images.RainWithSunIcon,"rainwithsun1.png"}
        };
        private readonly Dictionary<WeatherState, string> _weatherStateToWeatherBackgroundMapper =
            new Dictionary<WeatherState, string>(9)
        {
            {WeatherState.Cloudy,"cloudy1.jpg" },
            {WeatherState.CloudyWithSun,"cloudywithsun.jpg" },
            {WeatherState.Dull,"dull.jpg" },
            {WeatherState.Rain,"rain1.jpg" },
            {WeatherState.RainWithThunder,"rainwiththunder.jpg" },
            {WeatherState.Snowy,"snowy1.jpg" },
            {WeatherState.Sunny,"sunny1.jpg" },
            {WeatherState.ThunderWithoutRain,"cloudy1.jpg" }
        };
        private static readonly Dictionary<WeatherState, string> _weatherStateToWeatherIconMapper =
            new Dictionary<WeatherState, string>(9)
        {
            {WeatherState.Cloudy,"cloudy.png" },
            {WeatherState.CloudyWithSun,"cloudywithsun1.png" },
            {WeatherState.Dull,"dull1.png" },
            {WeatherState.Rain,"rain.png" },
            {WeatherState.RainWithThunder,"rainwiththunder1.png" },
            {WeatherState.Snowy,"snowy.png" },
            {WeatherState.Sunny,"sunny.png" },
            {WeatherState.ThunderWithoutRain,"dull1.png" }
        };
        private readonly Dictionary<string, string> _cityNameToCityPathBackgroundMapper =
     new Dictionary<string, string>
     {
        { "Rio-de-Janeiro","rio.jpg" },
        { "Moscow", "moscow.jpg" },
        { "Shanghai", "shanghai.jpg" },
        { "Obninsk", "obninsk.jpg" },
        { "Kamchatka", "kamchatka.jpg" },
        { "Paris", "paris.jpg" },
        { "Dubai", "dubai.jpg" },
        { "London", "london.jpg" },
        { "Pekin", "pekin.jpg" },
        { "Tokyo", "tokyo.jpg" },
        { "Sydney", "sydney.jpg" },
        { "Toronto", "toronto.jpg" },
        { "New-York", "newyork.jpg" },
        { "Rostov-On-Don", "rostovondon.jpg" },
        { "Delhi", "delhi.jpg" },
        { "Beijing", "beijing.png" },
        { "Istanbul", "istanbul.jpg" },
        { "Cairo", "cairo.jpg" },
        { "Dhaka", "dhaka.jpg" },
        { "Mexico-City", "mexicocity.jpg" },
        { "Seoul", "seoul.jpg" },
        { "Jakarta", "jakarta.jpg" },
        { "Bangkok", "bangkok.jpg" },
        { "Berlin", "berlin.jpg" },
        { "Madrid", "madrid.jpg" },
        { "Rome", "rome.jpg" },
        { "Washington", "washington.jpg" },
        { "Ottawa", "ottawa.jpg" },
        { "Canberra", "canberra.jpg" },
        { "Brasília", "brasilia.jpg" },
        { "Buenos-Aires", "buenosaires.jpg" },
        { "Lima", "lima.jpg" },
        { "Santiago", "santiago.jpg" },
        { "Bogotá", "bogota.jpg" },
        { "Caracas", "caracas.jpg" },
        { "Nairobi", "nairobi.jpg" },
        { "Pretoria", "pretoria.jpg" },
        { "Cape-Town", "capetown.jpg" },
        { "Abuja", "abuja.jpg" },
        { "Riyadh", "riyadh.jpg" },
        { "Tehran", "tehran.jpg" },
        { "Baghdad", "baghdad.jpg" },
        { "Kabul", "kabul.jpg" },
        { "Islamabad", "islamabad.jpg" },
        { "New-Delhi", "newdelhi.jpg" },
        { "Hanoi", "hanoi.jpg" },
        { "Manila", "manila.jpg" },
        { "Kuala-Lumpur", "kualalumpur.jpg" },
        { "Singapore", "singapore.jpg" },
        { "Vienna", "vienna.jpg" },
        { "Brussels", "brussels.jpg" },
        { "Amsterdam", "amsterdam.jpg" },
        { "Bern", "bern.jpg" },
        { "Stockholm", "stockholm.jpg" },
        { "Oslo", "oslo.jpg" },
        { "Helsinki", "helsinki.jpg" },
        { "Copenhagen", "copenhagen.jpg" },
        { "Reykjavik", "reykjavik.jpg" }
     };
        public async ValueTask<Stream> GetImageStream(Images intent)
        {//intent like song.mp3 -> page.BackgroundImageSource = ImageSource.FromStream(()=>res);
            return await FileSystem.OpenAppPackageFileAsync($"Resources/Images/{_intentToPathImagesMapper[intent]}");
        }
        public async ValueTask<Stream> GetWeatherIconStream(WeatherState state)
        {
            return await FileSystem.OpenAppPackageFileAsync($"Resources/Images/{_weatherStateToWeatherIconMapper[state]}");
        }
        public static string GetWeatherIconPath(WeatherState state)
        {
            return $"{_weatherStateToWeatherIconMapper[state]}";
        }
        public async ValueTask<Stream> GetWeatherBackgroundStream(WeatherState state)
        {
            return await FileSystem.OpenAppPackageFileAsync($"Resources/Images/{_weatherStateToWeatherBackgroundMapper[state]}");
        }
        public async ValueTask<Stream> GetCityBackgroundStream(string cityName)
        {
            return await FileSystem.OpenAppPackageFileAsync($"Resources/Images/{_cityNameToCityPathBackgroundMapper[cityName]}");
        }
        public async ValueTask<Stream> GetAudioWeatherStream(WeatherState state)
        {//intent like song.mp3
            return await FileSystem.OpenAppPackageFileAsync($"Resources/Audio/{_weatherStateToPathAudioMapper[state]}");
        }
    }
}
