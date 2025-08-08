using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Models;
using WeatherWhere.Services;
using WeatherWhere.ViewModels;
namespace WeatherWhere
{
    public partial class WeatherDetailsPage : ContentPage
    {
        public WeatherDetailsViewModel ViewModel { get; set; }
        private readonly IAudioImageSwitcher _switcher;
        public WeatherDetailsPage(WeatherDetailsViewModel viewModel,IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            ViewModel = viewModel;
            this.BindingContext = ViewModel;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            ViewModel.LoadFormattedInfo.Execute(null);
            await _switcher.SetBackgroundImage(this, false);
            base.OnAppearing();
        }
        public void UpdateData(WeatherListItemFiveDays item)
        {
            ViewModel.UpdateWeatherData(item);
        }
        public void UpdateData(CurrentWeatherData data)
        {
            ViewModel.UpdateWeatherData(data);
        }
    }
}