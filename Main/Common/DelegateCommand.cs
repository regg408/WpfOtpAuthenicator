using System.Windows.Input;

namespace Main.Common
{
    internal class DelegateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public Action<object?>? ExecuteAction { get; set; }
        public Func<object, bool>? CanExecuteFunc { get; set; }

        public bool CanExecute(object? parameter)
        {
            if (CanExecuteFunc == null)
            {
                return true;
            }
            return CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            if (ExecuteAction == null)
            {
                return;
            }
            ExecuteAction(parameter);
        }
    }
}
