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
    public class CurrentWeatherViewModel : INotifyPropertyChanged
    {
        public CurrentWeatherPage SourcePage;
        public FiveDaysWeatherPage FiveDaysWeatherPage;
        public WeatherDetailsPage WeatherDetailsPage;
        private readonly OpenWeatherMapAPI _api;
        private readonly IDisplayManager _errorHandler;
        private CurrentWeatherData _data;
        public CurrentWeatherData Data { get => _data; set { _data = value; OnPropertyChanged(); } }
        public ICommand GetCurrentData { get; set; }
        public ICommand GoToFiveDaysWeatherPage { get; set; }
        public ICommand GoToWeatherDetailsPage { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public CurrentWeatherViewModel(OpenWeatherMapAPI api, IDisplayManager errorHandler)
        {
            _api = api;
            _errorHandler = errorHandler;
            //команда в OnAppearing
            GetCurrentData = new GenericCommand(async (param) =>
            {
                try
                {
                    (CurrentWeatherData, bool) currentWeatherData = await _api.TryGetCurrentWeatherDataFromApiOrCache();
                    if (currentWeatherData.Item2)
                    {
                        Data = currentWeatherData.Item1;
                    }
                    else
                    {
                        Data = new CurrentWeatherData() { name = "ОШИБКА ПРИ ПОЛУЧЕНИИ ДАННЫХ!" };
                        _errorHandler.Display("ОШИБКА ПРИ ПОЛУЧЕНИИ ДАННЫХ С API!", DisplayIntent.Error, SourcePage);
                    }
                }
                catch (Exception ex)
                {

                }
            }, (param) => true);
            GoToFiveDaysWeatherPage = new GenericCommand(async (param) =>
            {
                await SourcePage.Navigation.PushAsync(FiveDaysWeatherPage);
            }, (param) => true);
            GoToWeatherDetailsPage = new GenericCommand(async (param) =>
            {
                WeatherDetailsPage.UpdateData(Data);
                await SourcePage.Navigation.PushAsync(WeatherDetailsPage);
            }, (param) => true);
        }
    }
}
