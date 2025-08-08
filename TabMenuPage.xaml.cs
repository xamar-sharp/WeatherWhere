using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWhere.Services;

namespace WeatherWhere
{
    public partial class TabMenuPage : TabbedPage
    {
        public TabMenuPage(
        CurrentWeatherPage currentWeatherPage,
        AboutUsPage aboutUsPage,
        CityInformationPage cityInformationPage,
        SettingsPage settingsPage,IAudioImageSwitcher switcher)
        {
            Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.TabbedPage.SetToolbarPlacement(this, Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);
            switcher.SetBackgroundAudio();
            Children.Add(new NavigationPage(currentWeatherPage) { IconImageSource = "weathermaintab.png",Title="Погода", BarBackgroundColor = Color.FromRgba("#2B579A"), BarTextColor = Colors.White });
            Children.Add(new NavigationPage(aboutUsPage) { IconImageSource = "aboutus.png", Title = "О нас", BarBackgroundColor = Colors.White, BarTextColor = Color.FromRgba("#333333") });
            Children.Add(new NavigationPage(cityInformationPage) { IconImageSource = "aboutcity.png", Title = "Город", BarBackgroundColor = Color.FromRgba("#121212"), BarTextColor = Colors.White });
            Children.Add(new NavigationPage(settingsPage) { IconImageSource = "settings.png", Title = "Настройки", BarBackgroundColor = Color.FromRgba("#6A3093"), BarTextColor = Colors.White });
        }      
    }
}