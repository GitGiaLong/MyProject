using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Core.Libraries.WPF.Interactivities
{
    [DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown")]
    [DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
    public abstract class TriggerAction : Animatable, IBehavior
    {

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof(IsEnabled), typeof(bool), 
            typeof(TriggerAction), new FrameworkPropertyMetadata(true));

        private DependencyObject associatedObject;
        protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();
                return associatedObject;
            }
        }

        private Type associatedObjectTypeConstraint;
        protected virtual Type AssociatedObjectTypeConstraint { get { ReadPreamble(); return associatedObjectTypeConstraint; } }

        private bool isHosted;
        internal bool IsHosted
        {
            get
            {
                ReadPreamble();
                return isHosted;
            }
            set
            {
                WritePreamble();
                isHosted = value;
                WritePostscript();
            }
        }

        internal TriggerAction(Type associatedObjectTypeConstraint)
        {
            this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
        }

        internal void CallInvoke(object parameter)
        {
            if (IsEnabled)
            {
                Invoke(parameter);
            }
        }

        protected abstract void Invoke(object parameter);

        public void Attach(DependencyObject dependencyObject)
        {
            if (dependencyObject != AssociatedObject)
            {
                if (AssociatedObject != null)
                {
                    throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
                }

                if (dependencyObject != null && !AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                        ExceptionStringTable.TypeConstraintViolatedExceptionMessage, GetType().Name,
                        dependencyObject.GetType().Name, AssociatedObjectTypeConstraint.Name));
                }

                WritePreamble();
                associatedObject = dependencyObject;
                WritePostscript();

                OnAttached();
            }
        }
        protected virtual void OnAttached() { }

        public void Detach()
        {
            OnDetaching();
            WritePreamble();
            associatedObject = null;
            WritePostscript();
        }
        protected virtual void OnDetaching() { }

        protected override Freezable CreateInstanceCore()
        {
            Type classType = GetType();
            return (Freezable)Activator.CreateInstance(classType);
        }

        DependencyObject IBehavior.AssociatedObject
        {
            get { return AssociatedObject; }
        }
    }
}
