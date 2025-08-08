using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;
using WeatherWhere.ViewModels;

namespace WeatherWhere
{
    public partial class CityPlacemarksPage : ContentPage
    {
        public CityPlacemarksViewModel ViewModel { get; set; }
        private readonly IAudioImageSwitcher _switcher;
        public CityPlacemarksPage(CityPlacemarksViewModel viewModel, IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            ViewModel = viewModel;
            this.BindingContext = ViewModel;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            await _switcher.SetBackgroundImage(this, true);
            await ViewModel.UpdateCityPlacemarksInfo();
            base.OnAppearing();
        }
    }
}