using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace Core.WPF.Interactivities
{
    public abstract class EventTriggerBase : TriggerBase
    {
        private Type sourceTypeConstraint;
        private bool isSourceChangedRegistered;
        private NameResolver sourceNameResolver;
        private MethodInfo eventHandlerMethodInfo;

        public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register("SourceObject",
                                                                                                    typeof(object),
                                                                                                    typeof(EventTriggerBase),
                                                                                                    new PropertyMetadata(
                                                                                                        new PropertyChangedCallback(OnSourceObjectChanged)));


        public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register("SourceName",
                                                                                                    typeof(string),
                                                                                                    typeof(EventTriggerBase),
                                                                                                    new PropertyMetadata(
                                                                                                        new PropertyChangedCallback(OnSourceNameChanged)));

        protected sealed override Type AssociatedObjectTypeConstraint
        {
            get
            {
                AttributeCollection attributes = TypeDescriptor.GetAttributes(GetType());
                TypeConstraintAttribute typeConstraintAttribute = attributes[typeof(TypeConstraintAttribute)] as TypeConstraintAttribute;

                if (typeConstraintAttribute != null)
                {
                    return typeConstraintAttribute.Constraint;
                }
                return typeof(DependencyObject);
            }
        }

        protected Type SourceTypeConstraint { get { return sourceTypeConstraint; } }

        public object SourceObject
        {
            get { return this.GetValue(SourceObjectProperty); }
            set { this.SetValue(SourceObjectProperty, value); }
        }

        public string SourceName
        {
            get { return (string)this.GetValue(SourceNameProperty); }
            set { this.SetValue(SourceNameProperty, value); }
        }

        public object Source
        {
            get
            {
                object source = this.AssociatedObject;

                if (SourceObject != null)
                {
                    source = SourceObject;
                }
                else if (IsSourceNameSet)
                {
                    source = SourceNameResolver.Object;
                    if (source != null && !SourceTypeConstraint.IsAssignableFrom(source.GetType()))
                    {
                        throw new InvalidOperationException(string.Format(
                            CultureInfo.CurrentCulture,
                            ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage,
                            GetType().Name,
                            source.GetType(),
                            SourceTypeConstraint,
                            "Source"));
                    }
                }
                return source;
            }
        }

        private NameResolver SourceNameResolver
        {
            get { return sourceNameResolver; }
        }

        private bool IsSourceChangedRegistered
        {
            get { return isSourceChangedRegistered; }
            set { isSourceChangedRegistered = value; }
        }

        private bool IsSourceNameSet
        {
            get
            {
                return !string.IsNullOrEmpty(SourceName) || this.ReadLocalValue(SourceNameProperty) != DependencyProperty.UnsetValue;
            }
        }

        private bool IsLoadedRegistered { get; set; }

        internal EventTriggerBase(Type sourceTypeConstraint) : base(typeof(DependencyObject))
        {
            this.sourceTypeConstraint = sourceTypeConstraint;
            sourceNameResolver = new NameResolver();
            RegisterSourceChanged();
        }

        protected abstract string GetEventName();

        protected virtual void OnEvent(EventArgs eventArgs)
        {
            this.InvokeActions(eventArgs);
        }

        private void OnSourceChanged(object oldSource, object newSource)
        {
            if (this.AssociatedObject != null)
            {
                OnSourceChangedImpl(oldSource, newSource);
            }
        }

        internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
        {
            if (string.IsNullOrEmpty(GetEventName()))
            {
                return;
            }
            if (string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) != 0)
            {
                if (oldSource != null && SourceTypeConstraint.IsAssignableFrom(oldSource.GetType()))
                {
                    UnregisterEvent(oldSource, GetEventName());
                }

                if (newSource != null && SourceTypeConstraint.IsAssignableFrom(newSource.GetType()))
                {
                    RegisterEvent(newSource, GetEventName());
                }
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            DependencyObject newHost = this.AssociatedObject;
            Behavior newBehavior = newHost as Behavior;
            FrameworkElement newHostElement = newHost as FrameworkElement;

            RegisterSourceChanged();
            if (newBehavior != null)
            {
                newHost = ((IBehavior)newBehavior).AssociatedObject;
                newBehavior.AssociatedObjectChanged += new EventHandler(OnBehaviorHostChanged);
            }
            else if (SourceObject != null || newHostElement == null)
            {
                try
                {
                    OnSourceChanged(null, Source);
                }
                catch (InvalidOperationException)
                {
                }
            }
            else
            {
                SourceNameResolver.NameScopeReferenceElement = newHostElement;
            }

            bool isLoadedEvent = string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0;

            if (isLoadedEvent && newHostElement != null && !Interaction.IsElementLoaded(newHostElement))
            {
                RegisterLoaded(newHostElement);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            Behavior oldBehavior = this.AssociatedObject as Behavior;
            FrameworkElement oldElement = this.AssociatedObject as FrameworkElement;

            try
            {
                OnSourceChanged(Source, null);
            }
            catch (InvalidOperationException)
            {
            }
            UnregisterSourceChanged();

            if (oldBehavior != null)
            {
                oldBehavior.AssociatedObjectChanged -= new EventHandler(OnBehaviorHostChanged);
            }

            SourceNameResolver.NameScopeReferenceElement = null;

            bool isLoadedEvent = string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0;

            if (isLoadedEvent && oldElement != null)
            {
                UnregisterLoaded(oldElement);
            }
        }

        private void OnBehaviorHostChanged(object sender, EventArgs e)
        {
            SourceNameResolver.NameScopeReferenceElement = ((IBehavior)sender).AssociatedObject as FrameworkElement;
        }

        private static void OnSourceObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EventTriggerBase eventTriggerBase = (EventTriggerBase)obj;
            object sourceNameObject = eventTriggerBase.SourceNameResolver.Object;
            if (args.NewValue == null)
            {
                eventTriggerBase.OnSourceChanged(args.OldValue, sourceNameObject);
            }
            else
            {
                if (args.OldValue == null && sourceNameObject != null)
                {
                    eventTriggerBase.UnregisterEvent(sourceNameObject, eventTriggerBase.GetEventName());
                }
                eventTriggerBase.OnSourceChanged(args.OldValue, args.NewValue);
            }
        }

        private static void OnSourceNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            EventTriggerBase trigger = (EventTriggerBase)obj;
            trigger.SourceNameResolver.Name = (string)args.NewValue;
        }

        private void RegisterSourceChanged()
        {
            if (!IsSourceChangedRegistered)
            {
                SourceNameResolver.ResolvedElementChanged += OnSourceNameResolverElementChanged;
                IsSourceChangedRegistered = true;
            }
        }

        private void UnregisterSourceChanged()
        {
            if (IsSourceChangedRegistered)
            {
                SourceNameResolver.ResolvedElementChanged -= OnSourceNameResolverElementChanged;
                IsSourceChangedRegistered = false;
            }
        }

        private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
        {
            if (SourceObject == null)
            {
                OnSourceChanged(e.OldObject, e.NewObject);
            }
        }

        private void RegisterLoaded(FrameworkElement associatedElement)
        {
            Debug.Assert(eventHandlerMethodInfo == null);
            Debug.Assert(!IsLoadedRegistered, "Trying to register Loaded more than once.");
            if (!IsLoadedRegistered && associatedElement != null)
            {
                associatedElement.Loaded += OnEventImpl;
                IsLoadedRegistered = true;
            }
        }

        protected void UnregisterLoaded(FrameworkElement associatedElement)
        {
            Debug.Assert(eventHandlerMethodInfo == null);
            if (IsLoadedRegistered && associatedElement != null)
            {
                associatedElement.Loaded -= OnEventImpl;
                IsLoadedRegistered = false;
            }
        }

        private void RegisterEvent(object obj, string eventName)
        {
            Debug.Assert(eventHandlerMethodInfo == null && string.Compare(eventName, "Loaded", StringComparison.Ordinal) != 0);
            Type targetType = obj.GetType();
            EventInfo eventInfo = targetType.GetEvent(eventName);
            if (eventInfo == null)
            {
                if (SourceObject != null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                                ExceptionStringTable.EventTriggerCannotFindEventNameExceptionMessage,
                                                                eventName,
                                                                obj.GetType().Name));
                }
                else
                {
                    return;
                }
            }

            if (!IsValidEvent(eventInfo))
            {
                if (SourceObject != null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                                                ExceptionStringTable.EventTriggerBaseInvalidEventExceptionMessage,
                                                                eventName,
                                                                obj.GetType().Name));
                }
                else
                {
                    return;
                }
            }
            eventHandlerMethodInfo = typeof(EventTriggerBase).GetMethod("OnEventImpl", BindingFlags.NonPublic | BindingFlags.Instance);
            eventInfo.AddEventHandler(obj, Delegate.CreateDelegate(eventInfo.EventHandlerType, this, eventHandlerMethodInfo));
        }

        private static bool IsValidEvent(EventInfo eventInfo)
        {
            Type eventHandlerType = eventInfo.EventHandlerType;
            if (typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
            {
                MethodInfo invokeMethod = eventHandlerType.GetMethod("Invoke");
                ParameterInfo[] parameters = invokeMethod.GetParameters();
                return parameters.Length == 2 && typeof(object).IsAssignableFrom(parameters[0].ParameterType) && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
            }
            return false;
        }

        private void UnregisterEvent(object obj, string eventName)
        {
            if (string.Compare(eventName, "Loaded", StringComparison.Ordinal) == 0)
            {
                FrameworkElement frameworkElement = obj as FrameworkElement;
                if (frameworkElement != null)
                {
                    UnregisterLoaded(frameworkElement);
                }
            }
            else
            {
                UnregisterEventImpl(obj, eventName);
            }
        }

        private void UnregisterEventImpl(object obj, string eventName)
        {
            Type targetType = obj.GetType();
            Debug.Assert(eventHandlerMethodInfo != null || targetType.GetEvent(eventName) == null);

            if (eventHandlerMethodInfo == null)
            {
                return;
            }

            EventInfo eventInfo = targetType.GetEvent(eventName);
            Debug.Assert(eventInfo != null, "Should not try to unregister an event that we successfully registered");
            eventInfo.RemoveEventHandler(obj, Delegate.CreateDelegate(eventInfo.EventHandlerType, this, eventHandlerMethodInfo));
            eventHandlerMethodInfo = null;
        }

        private void OnEventImpl(object sender, EventArgs eventArgs)
        {
            OnEvent(eventArgs);
        }

        internal void OnEventNameChanged(string oldEventName, string newEventName)
        {
            if (this.AssociatedObject != null)
            {
                FrameworkElement associatedElement = Source as FrameworkElement;

                if (associatedElement != null && string.Compare(oldEventName, "Loaded", StringComparison.Ordinal) == 0)
                {
                    UnregisterLoaded(associatedElement);
                }
                else if (!string.IsNullOrEmpty(oldEventName))
                {
                    UnregisterEvent(Source, oldEventName);
                }
                if (associatedElement != null && string.Compare(newEventName, "Loaded", StringComparison.Ordinal) == 0)
                {
                    RegisterLoaded(associatedElement);
                }
                else if (!string.IsNullOrEmpty(newEventName))
                {
                    RegisterEvent(Source, newEventName);
                }
            }
        }
    }
}
