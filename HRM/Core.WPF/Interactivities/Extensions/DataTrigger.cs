using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Core.WPF.Interactivities.Enums;

namespace Core.WPF.Interactivities.Extensions
{
    public class DataTrigger : PropertyChangedTrigger
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(DataTrigger), new PropertyMetadata(OnValueChanged));
        public static readonly DependencyProperty ComparisonProperty = DependencyProperty.Register("Comparison", typeof(ComparisonConditionType), typeof(DataTrigger), new PropertyMetadata(OnComparisonChanged));

        public object Value
        {
            get { return this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public ComparisonConditionType Comparison
        {
            get { return (ComparisonConditionType)this.GetValue(ComparisonProperty); }
            set { this.SetValue(ComparisonProperty, value); }
        }

        public DataTrigger() { }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is FrameworkElement element)
            {
                element.Loaded += OnElementLoaded;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            UnsubscribeElementLoadedEvent();
        }

        private void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                EvaluateBindingChange(e);
            }
            finally
            {
                UnsubscribeElementLoadedEvent();
            }
        }

        private void UnsubscribeElementLoadedEvent()
        {
            if (AssociatedObject is FrameworkElement element)
            {
                element.Loaded -= OnElementLoaded;
            }
        }

        protected override void EvaluateBindingChange(object args)
        {
            if (this.Compare())
            {
                this.InvokeActions(args);
            }
        }

        private static void OnValueChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            DataTrigger trigger = (DataTrigger)sender;
            trigger.EvaluateBindingChange(args);
        }

        private static void OnComparisonChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            DataTrigger trigger = (DataTrigger)sender;
            trigger.EvaluateBindingChange(args);
        }

        private bool Compare()
        {
            if (this.AssociatedObject != null)
            {
                return ComparisonLogic.EvaluateImpl(this.Binding, this.Comparison, this.Value);
            }
            return false;
        }
    }
}
