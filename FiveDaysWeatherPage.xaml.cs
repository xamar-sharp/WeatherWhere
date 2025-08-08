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
    public partial class FiveDaysWeatherPage : ContentPage
    {
        public FiveDaysWeatherViewModel ViewModel { get; set; }
        private readonly IAudioImageSwitcher _switcher;
        public FiveDaysWeatherPage(FiveDaysWeatherViewModel viewModel, WeatherDetailsPage page, IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            ViewModel = viewModel;
            ViewModel.SourcePage = this;
            ViewModel.WeatherDetailsPage = page;
            this.BindingContext = ViewModel;
            InitializeComponent();
        }
        public static bool Read = false;
        protected override async void OnAppearing()
        {
            if (!Read)
            {
                ViewModel.GetFiveDaysData.Execute(dataCol);
                Read = true;
            }
            await _switcher.SetBackgroundImage(this, false);
            scroll.InvalidateMeasure();
            base.OnAppearing();
        }

        private void dataCol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is WeatherListItemFiveDays selectedItem)
            {
                ViewModel.GoToWeatherDetailsPage.Execute(selectedItem);
            }
        }
    }
}