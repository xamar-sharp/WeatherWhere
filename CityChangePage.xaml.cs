using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;
using WeatherWhere.ViewModels;
namespace WeatherWhere
{
    public partial class CityChangePage : ContentPage
    {
        public ChangeCityViewModel ViewModel { get; set; }
        private readonly IAudioImageSwitcher _switcher;
        public CityChangePage(ChangeCityViewModel viewModel, IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            ViewModel = viewModel;
            viewModel.SourcePage = this;
            this.BindingContext = ViewModel;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            await _switcher.SetBackgroundImage(this, true);
            base.OnAppearing();
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedCity = e.CurrentSelection.FirstOrDefault().ToString();
            ViewModel.SelectedCityChangedCommand.Execute(null);
        }
    }
}