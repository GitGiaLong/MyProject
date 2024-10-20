﻿using System.Windows.Input;

namespace Core.Libraries.WPF.Extensions.CommandControls
{
    public class CloseWindowCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is DependencyObject dependencyObject)
            {
                if (Window.GetWindow(dependencyObject) is { } window)
                {
                    window.Close();
                }
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
