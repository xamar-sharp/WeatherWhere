using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;
using WeatherWhere.ViewModels;


namespace WeatherWhere
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsViewModel ViewModel { get; set; }
        private readonly IAudioImageSwitcher _switcher;
        public SettingsPage(SettingsViewModel viewModel, CityChangePage page,IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            ViewModel = viewModel;
            ViewModel.SourcePage = this;
            ViewModel.ChangeCityPage = page;
            this.BindingContext = ViewModel;
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await _switcher.SetBackgroundImage(this, false);
            base.OnAppearing();
        }
    }
}