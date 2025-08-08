using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using WeatherWhere.Services;
using System.Windows.Input;
using WeatherWhere.Models;
namespace WeatherWhere.ViewModels
{
    public class CityPlacemarksViewModel : INotifyPropertyChanged
    {
        private readonly IGeocodingService _geoService;
        private readonly ICityManager _cityManager;
        private string _placemarksInfo;
        public string PlacemarksInfo { get => _placemarksInfo; set { _placemarksInfo = value; OnPropertyChanged(); } }
        public ICommand GetPlacemarksCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public async ValueTask UpdateCityPlacemarksInfo()//Работает так как везде Singleton`s
        {
            CityInfo cityInfo = _cityManager.GetCityInfo(Application.Current.Resources["CurrentCityNameResource"].ToString());
            PlacemarksInfo = await _geoService.GetPlacemarksInfo(cityInfo.Longitude, cityInfo.Latitude);
        }
        public CityPlacemarksViewModel(IGeocodingService geoService, ICityManager cityManager)
        {
            _geoService = geoService;
            _cityManager = cityManager;
            //Команда вручную вызывается в OnAppearing();
            GetPlacemarksCommand = new GenericCommand(async (param) =>
            {
                await UpdateCityPlacemarksInfo();
            }, (param) => true);
        }
    }
}
