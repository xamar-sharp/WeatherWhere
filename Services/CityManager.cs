using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.ViewModels;

namespace WeatherWhere.Services
{
    public class CityInfo
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long Population { get; set; }

    }
    public interface ICityManager
    {
        CityInfo GetCityInfo(string cityName);
    }
    public class CityManager : ICityManager
    {
        private readonly Dictionary<string, CityInfo> _cityNameToDataMapper =
            new Dictionary<string, CityInfo>(12) {
{"Tokyo", new CityInfo(){ Latitude = 35.6897, Longitude = 139.6922, Population = 37274000 }},
{"Delhi", new CityInfo(){ Latitude = 28.6139, Longitude = 77.2090, Population = 32065760 }},
{"Beijing", new CityInfo(){ Latitude = 39.9042, Longitude = 116.4074, Population = 21516000 }},
{"Moscow", new CityInfo(){ Latitude = 55.7558, Longitude = 37.6176, Population = 12537954 }},
{"Istanbul", new CityInfo(){ Latitude = 41.0082, Longitude = 28.9784, Population = 15462452 }},
{"Cairo", new CityInfo(){ Latitude = 30.0444, Longitude = 31.2357, Population = 21750020 }},
{"Dhaka", new CityInfo(){ Latitude = 23.8103, Longitude = 90.4125, Population = 21741090 }},
{"Mexico-City", new CityInfo(){ Latitude = 19.4326, Longitude = -99.1332, Population = 9209944 }},
{"London", new CityInfo(){ Latitude = 51.5074, Longitude = -0.1278, Population = 8982000 }},
{"New-York", new CityInfo(){ Latitude = 40.7128, Longitude = -74.0060, Population = 8804190 }},
{"Seoul", new CityInfo(){ Latitude = 37.5665, Longitude = 126.9780, Population = 9976000 }},
{"Paris", new CityInfo(){ Latitude = 48.8566, Longitude = 2.3522, Population = 2161000 }},
{"Jakarta", new CityInfo(){ Latitude = -6.2088, Longitude = 106.8456, Population = 10562088 }},
{"Bangkok", new CityInfo(){ Latitude = 13.7563, Longitude = 100.5018, Population = 10539000 }},
{"Berlin", new CityInfo(){ Latitude = 52.5200, Longitude = 13.4050, Population = 3677000 }},
{"Madrid", new CityInfo(){ Latitude = 40.4168, Longitude = -3.7038, Population = 3223334 }},
{"Rome", new CityInfo(){ Latitude = 41.9028, Longitude = 12.4964, Population = 2873000 }},
{"Washington", new CityInfo(){ Latitude = 38.9072, Longitude = -77.0369, Population = 702455 }},
{"Ottawa", new CityInfo(){ Latitude = 45.4215, Longitude = -75.6972, Population = 1017449 }},
{"Canberra", new CityInfo(){ Latitude = -35.2809, Longitude = 149.1300, Population = 453558 }},
{"Brasília", new CityInfo(){ Latitude = -15.7942, Longitude = -47.8822, Population = 3094325 }},
{"Buenos-Aires", new CityInfo(){ Latitude = -34.6037, Longitude = -58.3816, Population = 15370000 }},
{"Lima", new CityInfo(){ Latitude = -12.0464, Longitude = -77.0428, Population = 10719188 }},
{"Santiago", new CityInfo(){ Latitude = -33.4489, Longitude = -70.6693, Population = 6257516 }},
{"Bogotá", new CityInfo(){ Latitude = 4.7110, Longitude = -74.0721, Population = 7963000 }},
{"Caracas", new CityInfo(){ Latitude = 10.4806, Longitude = -66.9036, Population = 2956000 }},
{"Nairobi", new CityInfo(){ Latitude = -1.2921, Longitude = 36.8219, Population = 4734881 }},
{"Pretoria", new CityInfo(){ Latitude = -25.7479, Longitude = 28.2293, Population = 2921488 }},
{"Cape-Town", new CityInfo(){ Latitude = -33.9249, Longitude = 18.4241, Population = 4710000 }},
{"Abuja", new CityInfo(){ Latitude = 9.0579, Longitude = 7.4951, Population = 3841000 }},
{"Riyadh", new CityInfo(){ Latitude = 24.7136, Longitude = 46.6753, Population = 7676654 }},
{"Tehran", new CityInfo(){ Latitude = 35.6892, Longitude = 51.3890, Population = 8693706 }},
{"Baghdad", new CityInfo(){ Latitude = 33.3152, Longitude = 44.3661, Population = 8126755 }},
{"Kabul", new CityInfo(){ Latitude = 34.5553, Longitude = 69.2075, Population = 4273156 }},
{"Islamabad", new CityInfo(){ Latitude = 33.6844, Longitude = 73.0479, Population = 1014825 }},
{"New-Delhi", new CityInfo(){ Latitude = 28.6139, Longitude = 77.2090, Population = 257803 }},
{"Hanoi", new CityInfo(){ Latitude = 21.0278, Longitude = 105.8342, Population = 8435700 }},
{"Manila", new CityInfo(){ Latitude = 14.5995, Longitude = 120.9842, Population = 13482000 }},
{"Kuala-Lumpur", new CityInfo(){ Latitude = 3.1390, Longitude = 101.6869, Population = 1808000 }},
{"Singapore", new CityInfo(){ Latitude = 1.3521, Longitude = 103.8198, Population = 5917600 }},
{"Vienna", new CityInfo(){ Latitude = 48.2082, Longitude = 16.3738, Population = 1920949 }},
{"Brussels", new CityInfo(){ Latitude = 50.8503, Longitude = 4.3517, Population = 1208542 }},
{"Amsterdam", new CityInfo(){ Latitude = 52.3676, Longitude = 4.9041, Population = 1154000 }},
{"Bern", new CityInfo(){ Latitude = 46.9480, Longitude = 7.4474, Population = 133883 }},
{"Stockholm", new CityInfo(){ Latitude = 59.3293, Longitude = 18.0686, Population = 975551 }},
{"Oslo", new CityInfo(){ Latitude = 59.9139, Longitude = 10.7522, Population = 697010 }},
{"Helsinki", new CityInfo(){ Latitude = 60.1699, Longitude = 24.9384, Population = 656920 }},
{"Copenhagen", new CityInfo(){ Latitude = 55.6761, Longitude = 12.5683, Population = 805402 }},
{"Reykjavik", new CityInfo(){ Latitude = 64.1466, Longitude = -21.9426, Population = 131136 }},
{"Shanghai", new CityInfo(){ Latitude = 31.2304, Longitude = 121.4737, Population = 26320000 }},
{"Obninsk", new CityInfo(){ Latitude = 55.0968, Longitude = 36.6101, Population = 115029 }},
{"Kamchatka", new CityInfo(){ Latitude = 53.0195, Longitude = 158.6469, Population = 291705 }},
{"Dubai", new CityInfo(){ Latitude = 25.2769, Longitude = 55.2962, Population = 3331000 }},
{"Sydney", new CityInfo(){ Latitude = -33.8688, Longitude = 151.2093, Population = 5312000 }},
{"Toronto", new CityInfo(){ Latitude = 43.6532, Longitude = -79.3832, Population = 2930000 }},
{"Rostov-On-Don", new CityInfo(){ Latitude = 47.2225, Longitude = 39.7187, Population = 1135000 }},
{"Rio-de-Janeiro", new CityInfo(){ Latitude = -22.9068, Longitude = -43.1729, Population = 6748000 }},
            };
        public static readonly List<string> AvailableCities = new List<string>
{
    "Rio-de-Janeiro",
    "Tokyo",
    "Delhi",
    "Beijing",
    "Moscow",
    "Istanbul",
    "Cairo",
    "Dhaka",
    "Mexico-City",
    "London",
    "New-York",
    "Seoul",
    "Paris",
    "Jakarta",
    "Bangkok",
    "Berlin",
    "Madrid",
    "Rome",
    "Washington",
    "Ottawa",
    "Canberra",
    "Brasília",
    "Buenos-Aires",
    "Lima",
    "Santiago",
    "Bogotá",
    "Caracas",
    "Nairobi",
    "Pretoria",
    "Cape-Town",
    "Abuja",
    "Riyadh",
    "Tehran",
    "Baghdad",
    "Kabul",
    "Islamabad",
    "New-Delhi",
    "Hanoi",
    "Manila",
    "Kuala-Lumpur",
    "Singapore",
    "Vienna",
    "Brussels",
    "Amsterdam",
    "Bern",
    "Stockholm",
    "Oslo",
    "Helsinki",
    "Copenhagen",
    "Reykjavik",
    "Shanghai",
    "Obninsk",
    "Kamchatka",
    "Dubai",
    "Sydney",
    "Toronto",
    "Rostov-On-Don"
};
        static CityManager()
        {
            if (Preferences.ContainsKey("CurrentCityName"))
            {
                Application.Current.Resources["CurrentCityNameResource"] = Preferences.Get("CurrentCityName", "Moscow");
            }
            else
            {
                Application.Current.Resources["CurrentCityNameResource"] = "Moscow";
            }
        }
        public static void SaveAndUpdateCurrentCityName(string newCityName)
        {
            Preferences.Set("CurrentCityName", newCityName);
            Application.Current.Resources["CurrentCityNameResource"] = newCityName;
            OpenWeatherMapAPI.WasChangedCity = true;
            FiveDaysWeatherPage.Read = false;
        }
        public CityInfo GetCityInfo(string cityName)
        {
            return _cityNameToDataMapper[cityName];
        }
    }
}
