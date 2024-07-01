using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Core.WPF.Interactivities
{
    [DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown")]
    [DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
    public abstract class TriggerAction : Animatable, IBehavior
    {
        private bool isHosted;
        private DependencyObject associatedObject;
        private Type associatedObjectTypeConstraint;

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(TriggerAction), new FrameworkPropertyMetadata(true));

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();
                return associatedObject;
            }
        }

        protected virtual Type AssociatedObjectTypeConstraint { get { ReadPreamble(); return associatedObjectTypeConstraint; } }

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
                                                                        ExceptionStringTable.TypeConstraintViolatedExceptionMessage,
                                                                        GetType().Name,
                                                                        dependencyObject.GetType().Name,
                                                                        AssociatedObjectTypeConstraint.Name));
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
            get
            {
                return AssociatedObject;
            }
        }
    }
}
