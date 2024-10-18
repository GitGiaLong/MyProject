using System.Collections;
using System.Globalization;

namespace Core.Libraries.WPF.Interactivities
{
    [CLSCompliant(false)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class DefaultTriggerAttribute : Attribute
    {
        private Type targetType;
        public Type TargetType
        {
            get { return targetType; }
        }

        private Type triggerType;
        public Type TriggerType
        {
            get { return triggerType; }
        }

        private object[] parameters;
        public IEnumerable Parameters
        {
            get { return parameters; }
        }

        public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter) : this(targetType, triggerType, new object[] { parameter })
        {
        }

        public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
        {
            if (!typeof(TriggerBase).IsAssignableFrom(triggerType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    ExceptionStringTable.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage, triggerType.Name));
            }


            this.targetType = targetType;
            this.triggerType = triggerType;
            this.parameters = parameters;
        }

        public TriggerBase Instantiate()
        {
            object trigger = null;
            try
            {
                trigger = Activator.CreateInstance(TriggerType, parameters);
            }
            catch
            {
            }
            return (TriggerBase)trigger;
        }
    }
}
