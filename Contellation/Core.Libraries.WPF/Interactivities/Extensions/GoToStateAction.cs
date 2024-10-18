using System.Globalization;

namespace Core.Libraries.WPF.Interactivities.Extensions
{
    public class GoToStateAction : TargetedTriggerAction<FrameworkElement>
    {
        public bool UseTransitions
        {
            get { return (bool)this.GetValue(UseTransitionsProperty); }
            set { this.SetValue(UseTransitionsProperty, value); }
        }
        public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register(nameof(UseTransitions), typeof(bool), typeof(GoToStateAction), new PropertyMetadata(true));

        public string StateName
        {
            get { return (string)this.GetValue(StateNameProperty); }
            set { this.SetValue(StateNameProperty, value); }
        }
        public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register(nameof(StateName), typeof(string), typeof(GoToStateAction), new PropertyMetadata(string.Empty));

        private FrameworkElement StateTarget { get; set; }

        private bool IsTargetObjectSet
        {
            get
            {
                bool isLocallySet = this.ReadLocalValue(TargetedTriggerAction.TargetObjectProperty) != DependencyProperty.UnsetValue;
                return isLocallySet;
            }
        }

        protected override void OnTargetChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
        {
            base.OnTargetChanged(oldTarget, newTarget);

            FrameworkElement frameworkElement = null;

            if (string.IsNullOrEmpty(this.TargetName) && !this.IsTargetObjectSet)
            {
                bool successful = VisualStateUtilities.TryFindNearestStatefulControl(this.AssociatedObject as FrameworkElement, out frameworkElement);
                if (!successful && frameworkElement != null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, 
                        ExceptionStringTable.GoToStateActionTargetHasNoStateGroups, frameworkElement.Name));
                }
            }
            else
            {
                frameworkElement = this.Target;
            }

            this.StateTarget = frameworkElement;
        }

        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject != null)
            {
                this.InvokeImpl(this.StateTarget);
            }
        }

        internal void InvokeImpl(FrameworkElement stateTarget)
        {
            if (stateTarget != null)
            {
                VisualStateUtilities.GoToState(stateTarget, this.StateName, this.UseTransitions);
            }
        }
    }
}
