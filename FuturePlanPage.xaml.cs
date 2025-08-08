using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;

namespace WeatherWhere
{
    public partial class FuturePlanPage : ContentPage
    {
        private readonly IAudioImageSwitcher _switcher;
        public FuturePlanPage(IAudioImageSwitcher switcher)
        {
            _switcher = switcher;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            await _switcher.SetBackgroundImage(this, false);
            base.OnAppearing();
        }
    }
}