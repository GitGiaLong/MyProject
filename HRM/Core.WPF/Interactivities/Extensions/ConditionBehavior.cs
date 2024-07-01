using System.Windows;
using System.Windows.Markup;
using Core.WPF.Interactivities.Interfaces;

namespace Core.WPF.Interactivities.Extensions
{
    [ContentProperty("Condition")]
    public class ConditionBehavior : Behavior<TriggerBase>
    {
        public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register("Condition", typeof(ICondition), typeof(ConditionBehavior), new System.Windows.PropertyMetadata(null));

        public ICondition Condition
        {
            get { return (ICondition)this.GetValue(ConditionProperty); }
            set { this.SetValue(ConditionProperty, value); }
        }

        public ConditionBehavior() { }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewInvoke += this.OnPreviewInvoke;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PreviewInvoke -= this.OnPreviewInvoke;
            base.OnDetaching();
        }

        private void OnPreviewInvoke(object sender, PreviewInvokeEventArgs e)
        {
            if (this.Condition != null)
            {
                e.Cancelling = !this.Condition.Evaluate();
            }
        }
    }
}