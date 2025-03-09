using Core.Libraries.WPF.Interactivities.Enums;
using System.Collections;
using System.Globalization;

namespace Core.Libraries.WPF.Interactivities.Extensions
{
    public class DataStateBehavior : Behavior<FrameworkElement>
    {
        public object Binding
        {
            get { return this.GetValue(BindingProperty); }
            set { this.SetValue(BindingProperty, value); }
        }
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(nameof(Binding), typeof(object), 
            typeof(DataStateBehavior), new PropertyMetadata(OnBindingChanged));

        public object Value
        {
            get { return this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), 
            typeof(DataStateBehavior), new PropertyMetadata(OnValueChanged));

        public string TrueState
        {
            get { return (string)this.GetValue(TrueStateProperty); }
            set { this.SetValue(TrueStateProperty, value); }
        }
        public static readonly DependencyProperty TrueStateProperty = DependencyProperty.Register(nameof(TrueState), typeof(string), 
            typeof(DataStateBehavior), new PropertyMetadata(OnTrueStateChanged));

        public string FalseState
        {
            get { return (string)this.GetValue(FalseStateProperty); }
            set { this.SetValue(FalseStateProperty, value); }
        }
        public static readonly DependencyProperty FalseStateProperty = DependencyProperty.Register(nameof(FalseState), typeof(string), 
            typeof(DataStateBehavior), new PropertyMetadata(OnFalseStateChanged));

        private FrameworkElement TargetObject
        {
            get
            {
                return VisualStateUtilities.FindNearestStatefulControl(this.AssociatedObject);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.ValidateStateNamesDeferred();
        }

        private void ValidateStateNamesDeferred()
        {
            FrameworkElement parentElement = this.AssociatedObject.Parent as FrameworkElement;

            if (parentElement != null && IsElementLoaded(parentElement))
            {
                this.ValidateStateNames();
            }
            else
            {
                this.AssociatedObject.Loaded += (o, e) =>
                {
                    this.ValidateStateNames();
                };
            }
        }

        internal static bool IsElementLoaded(FrameworkElement element)
        {
            return element.IsLoaded;
        }

        private void ValidateStateNames()
        {
            this.ValidateStateName(this.TrueState);
            this.ValidateStateName(this.FalseState);
        }

        private void ValidateStateName(string stateName)
        {
            if (this.AssociatedObject != null)
            {
                if (!string.IsNullOrEmpty(stateName))
                {
                    foreach (VisualState state in this.TargetedVisualStates)
                    {
                        if (stateName == state.Name)
                        {
                            return;
                        }
                    }
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                        ExceptionStringTable.DataStateBehaviorStateNameNotFoundExceptionMessage, stateName, this.TargetObject != null ?
                        this.TargetObject.GetType().Name : "null"));
                }
            }
        }

        private IEnumerable<VisualState> TargetedVisualStates
        {
            get
            {
                List<VisualState> states = new List<VisualState>();
                if (this.TargetObject != null)
                {
                    IList groups = VisualStateUtilities.GetVisualStateGroups(this.TargetObject);
                    foreach (VisualStateGroup group in groups)
                    {
                        foreach (VisualState state in group.States)
                        {
                            states.Add(state);
                        }
                    }
                }
                return states;
            }
        }

        private static void OnBindingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DataStateBehavior dataStateBehavior = (DataStateBehavior)obj;
            dataStateBehavior.Evaluate();
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DataStateBehavior dataStateBehavior = (DataStateBehavior)obj;
            dataStateBehavior.Evaluate();
        }

        private static void OnTrueStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DataStateBehavior dataStateBehavior = (DataStateBehavior)obj;
            dataStateBehavior.ValidateStateName(dataStateBehavior.TrueState);
            dataStateBehavior.Evaluate();
        }

        private static void OnFalseStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DataStateBehavior dataStateBehavior = (DataStateBehavior)obj;
            dataStateBehavior.ValidateStateName(dataStateBehavior.FalseState);
            dataStateBehavior.Evaluate();
        }

        private void Evaluate()
        {
            if (this.TargetObject != null)
            {
                string stateName = null;
                if (ComparisonLogic.EvaluateImpl(this.Binding, ComparisonConditionType.Equal, this.Value))
                {
                    stateName = this.TrueState;
                }
                else
                {
                    stateName = this.FalseState;
                }

                VisualStateUtilities.GoToState(this.TargetObject, stateName, true);
            }
        }
    }
}
