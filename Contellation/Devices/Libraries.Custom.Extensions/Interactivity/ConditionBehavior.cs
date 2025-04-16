using Libraries.Custom.Interactivities;
using Libraries.Custom.Interfaces.Interactivity;

namespace Libraries.Custom.Extensions.Interactivity
{
    [ContentProperty(nameof(Condition))]
    public class ConditionBehavior : Behavior<Interactivities.TriggerBase>
    {
        public ICondition Condition
        {
            get { return (ICondition)this.GetValue(ConditionProperty); }
            set { this.SetValue(ConditionProperty, value); }
        }
        public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register(nameof(Condition), typeof(ICondition), typeof(ConditionBehavior), new System.Windows.PropertyMetadata(null));


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
