using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;
using WeatherWhere.ViewModels;
namespace WeatherWhere
{
    public partial class CurrentWeatherPage : ContentPage
    {
        public CurrentWeatherViewModel ViewModel { get; set; }
        private readonly IAudioImageSwitcher _switcher;
        public CurrentWeatherPage(CurrentWeatherViewModel viewModel, WeatherDetailsPage wdp, FiveDaysWeatherPage fdwp,IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            ViewModel = viewModel;
            ViewModel.FiveDaysWeatherPage = fdwp;
            ViewModel.SourcePage = this;
            ViewModel.WeatherDetailsPage = wdp;
            this.BindingContext = ViewModel;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            ViewModel.GetCurrentData.Execute(null);
            weatherImg.Source = AssetManager.GetWeatherIconPath(OpenWeatherMapAPI.WeatherState);
            await _switcher.SetBackgroundImage(this, false);
            scroll.InvalidateMeasure();
            base.OnAppearing();
        }
    }
}