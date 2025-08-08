using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using WeatherWhere.Services;
using WeatherWhere.ViewModels;
namespace WeatherWhere
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            //Сначала регистрировать сервисы(от свободных к зависимым),затем ViewModel-s, затем только страницы(тоже от свободных к зависимым)
            //[У СТРАНИЦ И ВЬЮ-МОДЕЛЕЙ ЦИКЛ.СВЯЗЬ!-РЕШЕНО ЧЕРЕЗ ПУБЛ.УСТАНВОКА СВОЙСТВ-СТРАНИЦ]
            // builder.Services.AddSingleton<IAssetManager, AssetManager>(); ПОДКЛЮЧАЕМ ЗДЕСЬ DI(Страницы, ViewModels, Сервисы)
            builder.Services.AddSingleton<IAssetManager, AssetManager>();
            builder.Services.AddSingleton<IDisplayManager, DisplayManager>();
            builder.Services.AddSingleton<IAudioImageSwitcher, AudioImageSwitcher>();
            builder.Services.AddSingleton<ICityManager, CityManager>();
            builder.Services.AddSingleton<IGeocodingService, GeocodingService>();
            builder.Services.AddSingleton<OpenWeatherMapAPI>();
            builder.Services.AddSingleton<WeatherWhere.Services.INotificationService, WeatherWhere.Services.NotificationService>();
            builder.Services.AddSingleton<ChangeCityViewModel>();
            builder.Services.AddSingleton<CurrentCityInformationViewModel>();
            builder.Services.AddSingleton<CityPlacemarksViewModel>();
            builder.Services.AddSingleton<CurrentWeatherViewModel>();
            builder.Services.AddSingleton<FiveDaysWeatherViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<WeatherDetailsViewModel>();
            builder.Services.AddSingleton<CityChangePage>();
            builder.Services.AddSingleton<FiveDaysWeatherPage>();
            builder.Services.AddSingleton<WeatherDetailsPage>();
            builder.Services.AddSingleton<CityPlacemarksPage>();
            builder.Services.AddSingleton<FuturePlanPage>();
            builder.Services.AddSingleton<AboutUsPage>();
            builder.Services.AddSingleton<CityInformationPage>();
            builder.Services.AddSingleton<CurrentWeatherPage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<TabMenuPage>();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("OpenSans.ttf", "OpenSans");
                    fonts.AddFont("Roboto.ttf", "Roboto");
                    fonts.AddFont("HelveticaNeue.otf", "HelveticaNeue");
                }).UseLocalNotification();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
