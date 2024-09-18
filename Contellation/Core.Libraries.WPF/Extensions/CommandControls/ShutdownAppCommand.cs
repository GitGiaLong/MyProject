using System.Windows.Input;

namespace Core.Libraries.WPF.Extensions.CommandControls
{
    public class ShutdownAppCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Application.Current.Shutdown();

        public event EventHandler CanExecuteChanged;
    }
}
