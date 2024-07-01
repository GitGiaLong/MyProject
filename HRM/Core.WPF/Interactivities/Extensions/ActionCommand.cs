using System.Windows.Input;

namespace Core.WPF.Interactivities.Extensions
{
    public sealed class ActionCommand : ICommand
    {
        private Action action;
        private Action<object> objectAction;

        public ActionCommand(Action action) { this.action = action; }

        public ActionCommand(Action<object> objectAction)
        {
            this.objectAction = objectAction;
        }

        #region ICommand Members
#pragma warning disable 67
        private event EventHandler CanExecuteChanged;
#pragma warning restore 67

        event EventHandler ICommand.CanExecuteChanged
        {
            add { this.CanExecuteChanged += value; }
            remove { this.CanExecuteChanged -= value; }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (this.objectAction != null)
            {
                this.objectAction(parameter);
            }
            else
            {
                this.action();
            }
        }
        #endregion
    }
}
