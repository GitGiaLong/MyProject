using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;

namespace Core.WPF.Interactivities
{
    public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
    {
        private string commandName;

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), null);
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);
        public static readonly DependencyProperty EventArgsConverterProperty = DependencyProperty.Register("EventArgsConverter", typeof(IValueConverter), typeof(InvokeCommandAction), new PropertyMetadata(null));
        public static readonly DependencyProperty EventArgsConverterParameterProperty = DependencyProperty.Register("EventArgsConverterParameter", typeof(object), typeof(InvokeCommandAction), new PropertyMetadata(null));
        public static readonly DependencyProperty EventArgsParameterPathProperty = DependencyProperty.Register("EventArgsParameterPath", typeof(string), typeof(InvokeCommandAction), new PropertyMetadata(null));

        public string CommandName
        {
            get
            {
                ReadPreamble();
                return commandName;
            }
            set
            {
                if (CommandName != value)
                {
                    WritePreamble();
                    commandName = value;
                    WritePostscript();
                }
            }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter EventArgsConverter
        {
            get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
            set { SetValue(EventArgsConverterProperty, value); }
        }

        public object EventArgsConverterParameter
        {
            get { return GetValue(EventArgsConverterParameterProperty); }
            set { SetValue(EventArgsConverterParameterProperty, value); }
        }

        public string EventArgsParameterPath
        {
            get { return (string)GetValue(EventArgsParameterPathProperty); }
            set { SetValue(EventArgsParameterPathProperty, value); }
        }

        public bool PassEventArgsToCommand { get; set; }

        protected override void Invoke(object parameter)
        {
            if (AssociatedObject != null)
            {
                ICommand command = ResolveCommand();

                if (command != null)
                {
                    object commandParameter = CommandParameter;

                    //if no CommandParameter has been provided, let's check the EventArgsParameterPath
                    if (commandParameter == null && !string.IsNullOrWhiteSpace(EventArgsParameterPath))
                    {
                        commandParameter = GetEventArgsPropertyPathValue(parameter);
                    }

                    //next let's see if an event args converter has been supplied
                    if (commandParameter == null && EventArgsConverter != null)
                    {
                        commandParameter = EventArgsConverter.Convert(parameter, typeof(object), EventArgsConverterParameter, CultureInfo.CurrentCulture);
                    }

                    //last resort, let see if they want to force the event args to be passed as a parameter
                    if (commandParameter == null && PassEventArgsToCommand)
                    {
                        commandParameter = parameter;
                    }

                    if (command.CanExecute(commandParameter))
                    {
                        command.Execute(commandParameter);
                    }
                }
                else
                {
                    Debug.WriteLine(ExceptionStringTable.CommandDoesNotExistOnBehaviorWarningMessage, CommandName, AssociatedObject.GetType().Name);
                }
            }
        }

        private object GetEventArgsPropertyPathValue(object parameter)
        {
            object commandParameter;
            object propertyValue = parameter;
            string[] propertyPathParts = EventArgsParameterPath.Split('.');
            foreach (string propertyPathPart in propertyPathParts)
            {
                PropertyInfo propInfo = propertyValue.GetType().GetProperty(propertyPathPart);
                propertyValue = propInfo.GetValue(propertyValue, null);
            }

            commandParameter = propertyValue;
            return commandParameter;
        }

        private ICommand ResolveCommand()
        {
            ICommand command = null;

            if (Command != null)
            {
                command = Command;
            }
            else if (AssociatedObject != null)
            {
                // todo jekelly 06/09/08: we could potentially cache some or all of this information if needed, updating when AssociatedObject changes
                Type associatedObjectType = AssociatedObject.GetType();
                PropertyInfo[] typeProperties = associatedObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo propertyInfo in typeProperties)
                {
                    if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        if (string.Equals(propertyInfo.Name, CommandName, StringComparison.Ordinal))
                        {
                            command = (ICommand)propertyInfo.GetValue(AssociatedObject, null);
                        }
                    }
                }
            }

            return command;
        }
    }
}
