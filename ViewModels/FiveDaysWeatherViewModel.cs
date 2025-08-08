using Microsoft.Maui.Controls;
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
    public class FiveDaysWeatherViewModel
    {
        public FiveDaysWeatherPage SourcePage;
        public WeatherDetailsPage WeatherDetailsPage;
        private readonly OpenWeatherMapAPI _api;
        private readonly IDisplayManager _errorHandler;
        private WeatherDataFiveDays _data;
        public WeatherDataFiveDays Data { get => _data; set { _data = value; OnPropertyChanged(); } }
        public ICommand GetFiveDaysData { get; set; }
        public ICommand GoToWeatherDetailsPage { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public FiveDaysWeatherViewModel(OpenWeatherMapAPI api, IDisplayManager errorHandler)
        {
            _api = api;
            _errorHandler = errorHandler;
            //команда в OnAppearing
            GetFiveDaysData = new GenericCommand(async (param) =>
            {
                (WeatherDataFiveDays, bool) weatherDataFiveDays = await _api.TryGetFiveDaysWeatherDataFromApiOrCache();
                CollectionView collectionView = param as CollectionView;
                if (weatherDataFiveDays.Item2)
                {
                    Data = weatherDataFiveDays.Item1;
                    collectionView.ItemsSource = null;
                    collectionView.ItemsSource = Data.List;                  
                }
                else
                {
                    Data = new WeatherDataFiveDays() { Cod = "ОШИБКА ПРИ ПОЛУЧЕНИИ ДАННЫХ!" };
                    _errorHandler.Display("ОШИБКА ПРИ ПОЛУЧЕНИИ ДАННЫХ С API!", DisplayIntent.Error, SourcePage);
                }
            }, (param) => true);
            GoToWeatherDetailsPage = new GenericCommand(async (param) =>
            {
                WeatherDetailsPage.UpdateData(param as WeatherListItemFiveDays);
                await SourcePage.Navigation.PushAsync(WeatherDetailsPage);//передавать данные на WeatherDetailsPage через здесь изменение ее свойств через вызов ее метода отсюда
            }, (param) => true);
        }
    }
}
