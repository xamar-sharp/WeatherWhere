using Microsoft.Maui.Storage;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Services
{
    public interface IAudioImageSwitcher
    {//СИНГЛТОН КАК И ВСЕ СЕРВИСЫ!
        Task SetBackgroundImage(Page page, bool isCityBackground);//ориентируясь на CurrentCityName(CityService)(+на IsNight или WeatherState)
        void SetVolume(double volume);//установить громкость(0-1.0)
        void SetBackgroundAudio();//ориентируясь на WeatherState(Api)
    }
    public class AudioImageSwitcher : IAudioImageSwitcher
    {
        private readonly IAssetManager _assetManager;
        private readonly IDisplayManager _errorManager;
        private IAudioManager _audioManager;
        private IAudioPlayer _player;
        public AudioImageSwitcher(IAssetManager assetManager, IDisplayManager errorManager)
        {
            _assetManager = assetManager;
            _errorManager = errorManager;
            _audioManager = AudioManager.Current;
        }
        public async Task SetBackgroundImage(Page page, bool isCityBackground)
        {
            try
            {
                if (isCityBackground)
                {
                    var stream = await _assetManager.GetCityBackgroundStream(Application.Current.Resources["CurrentCityNameResource"].ToString());
                    page.BackgroundImageSource = ImageSource.FromStream(() => stream);
                }
                else
                {
                    if (OpenWeatherMapAPI.IsNight)
                    {
                        var stream = await _assetManager.GetImageStream(Images.NightBack);
                        page.BackgroundImageSource = ImageSource.FromStream(() => stream);
                    }
                    else
                    {
                        var stream = await _assetManager.GetWeatherBackgroundStream(OpenWeatherMapAPI.WeatherState);
                        page.BackgroundImageSource = ImageSource.FromStream(() => stream);
                    }
                }
            }
            catch (Exception ex)
            {
                //ОБРАБОТКА ОШИБОК
                _errorManager.Display(ex.Message, DisplayIntent.Error, page);
            }
        }
        public void SetBackgroundAudio()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (_player != null)
                {
                    _player.Stop();
                    _player.Dispose();
                }
                var stream = await _assetManager.GetAudioWeatherStream(OpenWeatherMapAPI.WeatherState);
                _player = _audioManager.CreatePlayer(stream);
                _player.Loop = true;
                _player.Play();
            });
        }
        public void SetVolume(double volume)
        {
            if (_player != null)
            {
                volume = Math.Clamp(volume, 0, 1); // Предотвращаем выход за границы
                _player.Volume = volume;
            }
        }
    }
}
