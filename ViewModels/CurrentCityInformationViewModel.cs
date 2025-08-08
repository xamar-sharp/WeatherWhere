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
    public class CurrentCityInformationViewModel : INotifyPropertyChanged
    {
        private double _longitude;
        public double Longitude { get => _longitude; set { _longitude = value; OnPropertyChanged(); } }
        private double _latitude;
        public double Latitude { get => _latitude; set { _latitude = value; OnPropertyChanged(); } }
        private long _population;
        public long Population { get => _population; set { _population = value; OnPropertyChanged(); } }
        public ICommand GoToPlacemarksPageCommand { get; set; }
        public CityInformationPage SourcePage;
        public CityPlacemarksPage PlacemarksPage;
        private readonly ICityManager _cityManager;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void UpdateCityPageData()
        {
            CityInfo cityInfo = _cityManager.GetCityInfo(Application.Current.Resources["CurrentCityNameResource"].ToString());
            Longitude = cityInfo.Longitude;
            Latitude = cityInfo.Latitude;
            Population = cityInfo.Population;
        }
        public CurrentCityInformationViewModel(ICityManager cityManager)
        {
            _cityManager = cityManager;
            UpdateCityPageData();
            GoToPlacemarksPageCommand = new GenericCommand(async (param) =>
            {
                await SourcePage.Navigation.PushAsync(PlacemarksPage);
            },(param)=>true);
        }
    }
}
