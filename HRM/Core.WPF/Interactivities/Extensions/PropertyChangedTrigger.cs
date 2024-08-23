namespace Core.WPF.Interactivities.Extensions
{
    public class PropertyChangedTrigger : TriggerBase<DependencyObject>
    {
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(object), typeof(PropertyChangedTrigger), new PropertyMetadata(OnBindingChanged));

        public object Binding
        {
            get { return (object)this.GetValue(BindingProperty); }
            set { this.SetValue(BindingProperty, value); }
        }

        protected virtual void EvaluateBindingChange(object args)
        {
            this.InvokeActions(args);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.PreviewInvoke += this.OnPreviewInvoke;
        }

        protected override void OnDetaching()
        {
            this.PreviewInvoke -= this.OnPreviewInvoke;
            this.OnDetaching();
        }

        void OnPreviewInvoke(object sender, PreviewInvokeEventArgs e)
        {
            DataBindingHelper.EnsureDataBindingOnActionsUpToDate(this);
        }

        private static void OnBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PropertyChangedTrigger propertyChangedTrigger = (PropertyChangedTrigger)sender;
            propertyChangedTrigger.EvaluateBindingChange(args);
        }

    }
}
