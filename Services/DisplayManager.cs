using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Services
{
    public enum DisplayIntent
    {
        Error,
        Information,
        Warning
    }
    public interface IDisplayManager
    {
        void Display(string msg, DisplayIntent intent, Page sourcePage);
    }
    public class DisplayManager : IDisplayManager//в том числе для обработки и вывода ошибок
    {
        public void Display(string msg, DisplayIntent intent, Page sourcePage)
        {
            string title = intent switch
            {
                DisplayIntent.Error => "Ошибка",
                DisplayIntent.Warning => "Предупреждение",
                _ => "Информация"
            };
            MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await sourcePage.DisplayAlert(title, msg, "Закрыть");
                });
        }
    }
}
