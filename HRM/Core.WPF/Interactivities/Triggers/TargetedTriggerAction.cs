using System.ComponentModel;
using System.Globalization;

namespace Core.WPF.Interactivities
{
    public abstract class TargetedTriggerAction : TriggerAction
    {
        private Type targetTypeConstraint;
        private bool isTargetChangedRegistered;
        private NameResolver targetResolver;

        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject",
                                                                                                    typeof(object),
                                                                                                    typeof(TargetedTriggerAction),
                                                                                                     new FrameworkPropertyMetadata(
                                                                                                        new PropertyChangedCallback(OnTargetObjectChanged)));

        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName",
                                                                                                    typeof(string),
                                                                                                    typeof(TargetedTriggerAction),
                                                                                                    new FrameworkPropertyMetadata(
                                                                                                        new PropertyChangedCallback(OnTargetNameChanged)));

        public object TargetObject
        {
            get { return GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        public string TargetName
        {
            get { return (string)GetValue(TargetNameProperty); }
            set { SetValue(TargetNameProperty, value); }
        }

        protected object Target
        {
            get
            {
                object target = AssociatedObject;
                if (TargetObject != null)
                {
                    target = TargetObject;
                }
                else if (IsTargetNameSet)
                {
                    target = TargetResolver.Object;
                }

                if (target != null && !TargetTypeConstraint.IsAssignableFrom(target.GetType()))
                {
                    throw new InvalidOperationException(string.Format(
                        CultureInfo.CurrentCulture,
                        ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage,
                        GetType().Name,
                        target.GetType(),
                        TargetTypeConstraint,
                        "Target"));
                }
                return target;
            }
        }

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

        protected Type TargetTypeConstraint
        {
            get
            {
                ReadPreamble();
                return targetTypeConstraint;
            }
        }

        private bool IsTargetNameSet
        {
            get
            {
                return !string.IsNullOrEmpty(TargetName) || ReadLocalValue(TargetNameProperty) != DependencyProperty.UnsetValue;
            }
        }

        private NameResolver TargetResolver
        {
            get { return targetResolver; }
        }

        private bool IsTargetChangedRegistered
        {
            get { return isTargetChangedRegistered; }
            set { isTargetChangedRegistered = value; }
        }

        internal TargetedTriggerAction(Type targetTypeConstraint) : base(typeof(DependencyObject))
        {
            this.targetTypeConstraint = targetTypeConstraint;
            targetResolver = new NameResolver();
            RegisterTargetChanged();
        }

        internal virtual void OnTargetChangedImpl(object oldTarget, object newTarget) { }

        protected override void OnAttached()
        {
            base.OnAttached();
            DependencyObject hostObject = AssociatedObject;
            Behavior newBehavior = hostObject as Behavior;

            RegisterTargetChanged();
            if (newBehavior != null)
            {
                hostObject = ((IBehavior)newBehavior).AssociatedObject;
                newBehavior.AssociatedObjectChanged += new EventHandler(OnBehaviorHostChanged);
            }
            TargetResolver.NameScopeReferenceElement = hostObject as FrameworkElement;
        }

        protected override void OnDetaching()
        {
            Behavior oldBehavior = AssociatedObject as Behavior;
            base.OnDetaching();
            OnTargetChangedImpl(TargetResolver.Object, null);
            UnregisterTargetChanged();

            if (oldBehavior != null)
            {
                oldBehavior.AssociatedObjectChanged -= new EventHandler(OnBehaviorHostChanged);
            }
            TargetResolver.NameScopeReferenceElement = null;
        }

        private void OnBehaviorHostChanged(object sender, EventArgs e)
        {
            TargetResolver.NameScopeReferenceElement = ((IBehavior)sender).AssociatedObject as FrameworkElement;
        }

        private void RegisterTargetChanged()
        {
            if (!IsTargetChangedRegistered)
            {
                TargetResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(OnTargetChanged);
                IsTargetChangedRegistered = true;
            }
        }

        private void UnregisterTargetChanged()
        {
            if (IsTargetChangedRegistered)
            {
                TargetResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(OnTargetChanged);
                IsTargetChangedRegistered = false;
            }
        }

        private static void OnTargetObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
            targetedTriggerAction.OnTargetChanged(obj, new NameResolvedEventArgs(args.OldValue, args.NewValue));
        }

        private static void OnTargetNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
            targetedTriggerAction.TargetResolver.Name = (string)args.NewValue;
        }

        private void OnTargetChanged(object sender, NameResolvedEventArgs e)
        {
            if (AssociatedObject != null)
            {
                OnTargetChangedImpl(e.OldObject, e.NewObject);
            }
        }
    }
}
