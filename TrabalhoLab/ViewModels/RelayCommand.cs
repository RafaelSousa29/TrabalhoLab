using System;
using System.Windows.Input;

namespace TrabalhoLab.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executar;
        private readonly Func<bool>? _podeExecutar;

        public RelayCommand(Action executar, Func<bool>? podeExecutar = null)
        {
            _executar = executar;
            _podeExecutar = podeExecutar;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _podeExecutar?.Invoke() ?? true;

        public void Execute(object? parameter) => _executar();
    }
}
