using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeatherWhere.Models
{
    public class GenericCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public GenericCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public void Execute(object? param)
        {
            _execute(param);
        }
        public bool CanExecute(object? param)
        {
            return _canExecute(param);
        }
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
}
