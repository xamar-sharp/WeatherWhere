using System.Diagnostics;
using Plugin.Maui.Audio;
namespace WeatherWhere
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;
        public App(IServiceProvider services)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Debug.WriteLine((e?.ExceptionObject as Exception)?.StackTrace);
                Debug.WriteLine("UnhandledException: " + e.ExceptionObject?.ToString());
            };
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Debug.WriteLine("UnobservedTaskException: " + e.Exception?.ToString());
                Debug.WriteLine(e?.Exception?.StackTrace);
                e.SetObserved();
            };
            _services = services;
            InitializeComponent();
        }
        protected override async void OnStart()
        {

            var notificationsStatus = await Permissions.RequestAsync<Permissions.PostNotifications>();
            if (!(notificationsStatus == PermissionStatus.Granted || notificationsStatus == PermissionStatus.Limited))
            {
                Environment.Exit(0);
                Process.GetCurrentProcess().Kill();
                return;
            }
            base.OnStart();
        }
        protected override Window CreateWindow(IActivationState? activationState)
        { //TabMenuPage тоже надо внедрять через DI
            var tabMenuPage = _services.GetRequiredService<TabMenuPage>();
            return new Window(tabMenuPage);
        }
    }
}
