using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherWhere.Models;
using WeatherWhere.Services;
namespace WeatherWhere.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private bool _notificationsEnabled;
        private double _volume;
        private int _titleFontSize;
        private int _textFontSize;
        public SettingsPage SourcePage;
        public CityChangePage ChangeCityPage;
        private readonly IAudioImageSwitcher _switcher;
        private readonly INotificationService _notificator;
        private readonly IDisplayManager _display;
        private readonly IAssetManager _assetManager;
        public bool NotificationsEnabled
        {
            get => _notificationsEnabled; set
            {
                _notificationsEnabled = value;
                OnPropertyChanged();
            }
        }
        public double Volume
        {
            get => _volume; set
            {
                _volume = value;
                OnPropertyChanged();
                this._switcher.SetVolume(Volume);
            }
        }
        public string TitleFontSize
        {
            get => _titleFontSize.ToString(); set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                _titleFontSize = Int32.Parse(value);
                OnPropertyChanged();
                Application.Current.Resources["TitleFontSize"] = (double)_titleFontSize;
            }
        }
        public string TextFontSize
        {
            get => _textFontSize.ToString(); set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                _textFontSize = Int32.Parse(value);
                OnPropertyChanged();
                Application.Current.Resources["TextFontSize"] = (double)_textFontSize;
            }
        }
        public ICommand MuteCommand { get; set; }
        public ICommand NotificationsStateChangedCommand { get; set; }
        public ICommand GoToChangeCityPageCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public SettingsViewModel(IAudioImageSwitcher switcher, INotificationService notificator, IDisplayManager display, IAssetManager assetManager)//ИЗМЕНИТЬ НА СТРАНИЦУ С НАСТРОЙКАМИ
        {
            TextFontSize = "16";
            TitleFontSize = "22";
            _switcher = switcher;
            _display = display;
            _notificator = notificator;
            _assetManager = assetManager;
            NotificationsEnabled = true;
            MuteCommand = new GenericCommand((param) =>
            {
                Volume = 0;
            }, (param) => true);
            NotificationsStateChangedCommand = new GenericCommand(async (param) =>
            {
                try
                {
                    bool res;
                    if (NotificationsEnabled)
                    {
                        res = await _notificator.EnableNotifications();
                        var stream = await _assetManager.GetImageStream(Images.DisableNotifications);
                        (param as Microsoft.Maui.Controls.Image).Source = ImageSource.FromStream(() => stream);
                        NotificationsEnabled = false;
                        return;
                    }
                    else
                    {
                        res = _notificator.DisableNotifications();
                        var stream = await _assetManager.GetImageStream(Images.EnableNotifications);
                        (param as Microsoft.Maui.Controls.Image).Source = ImageSource.FromStream(() => stream);
                        NotificationsEnabled = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _display.Display("Уведомления не удалось включить/отключить!", DisplayIntent.Error, SourcePage);
                }
            }, (obj) => true);
            GoToChangeCityPageCommand = new GenericCommand(async (param) =>
            {
                await SourcePage.Navigation.PushAsync(ChangeCityPage);
            }, (obj) => true);
        }
    }
}
