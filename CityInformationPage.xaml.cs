using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;
using WeatherWhere.ViewModels;
namespace WeatherWhere
{
    public partial class CityInformationPage : ContentPage
    {
        public CurrentCityInformationViewModel ViewModel { get; set; }
        private readonly IAudioImageSwitcher _switcher;
        public CityInformationPage(CurrentCityInformationViewModel viewModel,CityPlacemarksPage plPage,IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            ViewModel = viewModel;
            ViewModel.SourcePage = this;
            ViewModel.PlacemarksPage = plPage;
            this.BindingContext = ViewModel;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            await _switcher.SetBackgroundImage(this, true);
            base.OnAppearing();
        }
    }
}