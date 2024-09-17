using System.Windows.Input;

namespace Core.WPF.Controls.Extensions
{
    public class StartScreenshotCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => new Screenshot().Start();

        public event EventHandler CanExecuteChanged;
    }
}
