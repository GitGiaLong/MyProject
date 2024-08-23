using System.Globalization;
using System.Windows.Media.Animation;

namespace Core.WPF.Interactivities
{
    [ContentProperty("Actions")]
    public abstract class TriggerBase : Animatable, IBehavior
    {
        private DependencyObject associatedObject;
        private Type associatedObjectTypeConstraint;

        private static readonly DependencyPropertyKey ActionsPropertyKey = DependencyProperty.RegisterReadOnly("Actions", typeof(TriggerActionCollection), typeof(TriggerBase), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty ActionsProperty = ActionsPropertyKey.DependencyProperty;

        internal TriggerBase(Type associatedObjectTypeConstraint)
        {
            this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
            TriggerActionCollection newCollection = new TriggerActionCollection();
            SetValue(ActionsPropertyKey, newCollection);
        }

        protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();
                return associatedObject;
            }
        }

        protected virtual Type AssociatedObjectTypeConstraint
        {
            get
            {
                ReadPreamble();
                return associatedObjectTypeConstraint;
            }
        }

        public TriggerActionCollection Actions
        {
            get
            {
                return (TriggerActionCollection)GetValue(ActionsProperty);
            }
        }

        public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

        protected void InvokeActions(object parameter)
        {
            if (PreviewInvoke != null)
            {
                PreviewInvokeEventArgs previewInvokeEventArg = new PreviewInvokeEventArgs();
                PreviewInvoke(this, previewInvokeEventArg);
                if (previewInvokeEventArg.Cancelling == true)
                {
                    return;
                }
            }

            foreach (TriggerAction action in Actions)
            {
                action.CallInvoke(parameter);
            }
        }

        DependencyObject IBehavior.AssociatedObject
        {
            get
            {
                return AssociatedObject;
            }
        }

        public void Attach(DependencyObject dependencyObject)
        {
            if (dependencyObject != AssociatedObject)
            {
                if (AssociatedObject != null)
                {
                    throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerMultipleTimesExceptionMessage);
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

                Actions.Attach(dependencyObject);
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
            Actions.Detach();
        }
        protected virtual void OnDetaching() { }

        protected override Freezable CreateInstanceCore()
        {
            Type classType = GetType();
            return (Freezable)Activator.CreateInstance(classType);
        }

    }
}
