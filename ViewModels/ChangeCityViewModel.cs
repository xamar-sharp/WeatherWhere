using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WeatherWhere.Services;
using WeatherWhere.Models;
namespace WeatherWhere.ViewModels
{
    public class ChangeCityViewModel : INotifyPropertyChanged
    {
        private object _selectedCity;
        public object SelectedCity { get => _selectedCity; set { _selectedCity = value; OnPropertyChanged(); } }
        private string _searchText;
        public string SearchText { get => _searchText; set { _searchText = value; OnPropertyChanged(); } }
        private List<object> _allCities;
        public List<object> AllCities { get => _allCities; set { _allCities = value; OnPropertyChanged(); } }
        public ICommand SelectedCityChangedCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        private readonly IDisplayManager _errorHandler;
        private readonly IAudioImageSwitcher _switcher;
        private readonly CurrentCityInformationViewModel _viewModel;
        public CityChangePage SourcePage;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public ChangeCityViewModel(IDisplayManager errorHandler, IAudioImageSwitcher switcher,CurrentCityInformationViewModel viewModel)
        {
            _viewModel = viewModel;
            _switcher = switcher;
            _errorHandler = errorHandler;
            AllCities = CityManager.AvailableCities.Select(e => (object)e).ToList();
            SelectedCity = Application.Current.Resources["CurrentCityNameResource"].ToString();
            SelectedCityChangedCommand = new GenericCommand((param) =>
            {
                if (SelectedCity.ToString() == Application.Current.Resources["CurrentCityNameResource"].ToString())
                {
                    return;
                }
                CityManager.SaveAndUpdateCurrentCityName(SelectedCity.ToString());
                _viewModel.UpdateCityPageData();
                _switcher.SetBackgroundImage(SourcePage, true);
            }, (param) => true);
            SearchCommand = new GenericCommand((param) =>
            {
                object city = AllCities.FirstOrDefault(e => e.ToString().Contains(SearchText));
                if (city is null)
                {
                    _errorHandler.Display("ГОРОД НЕ НАЙДЕН!", DisplayIntent.Error, SourcePage);
                    return;
                }
                SelectedCity = city.ToString();
                SelectedCityChangedCommand?.Execute(null);
            }, (param) => true);
        }
    }
}
