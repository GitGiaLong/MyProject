﻿using System.Collections;
using System.Globalization;

namespace Core.WPF.Interactivities
{
    [CLSCompliant(false)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class DefaultTriggerAttribute : Attribute
    {
        private Type targetType;
        private Type triggerType;
        private object[] parameters;

        public Type TargetType
        {
            get { return targetType; }
        }

        public Type TriggerType
        {
            get { return triggerType; }
        }

        public IEnumerable Parameters
        {
            get { return parameters; }
        }

        public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter) :
            this(targetType, triggerType, new object[] { parameter })
        {
        }

        public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
        {
            if (!typeof(TriggerBase).IsAssignableFrom(triggerType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                            ExceptionStringTable.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage,
                                                            triggerType.Name));
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
