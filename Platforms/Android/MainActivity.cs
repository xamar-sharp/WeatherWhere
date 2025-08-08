using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
namespace WeatherWhere
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("Android Unhandled Exception: " + e.Exception);
                e.Handled = true; // Если хочешь, чтобы приложение не падало
            };
        }
    }
}
