using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;
namespace WeatherWhere
{
    public partial class AboutUsPage : ContentPage
    {
        private readonly FuturePlanPage _page;
        private readonly IAudioImageSwitcher _switcher;
        public AboutUsPage(FuturePlanPage page,IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            _page = page;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            await _switcher.SetBackgroundImage(this, false);
            base.OnAppearing();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_page);
        }
    }
}