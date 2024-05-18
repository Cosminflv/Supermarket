using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.Commands
{
    public class RelaySearchCommand : ICommand
    {
        private readonly Action<object[]> _execute;
        private readonly Predicate<object[]> _canExecute;

        public RelaySearchCommand(Action<object[]> execute, Predicate<object[]> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((object[])parameter);

        public void Execute(object parameter) => _execute((object[])parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
