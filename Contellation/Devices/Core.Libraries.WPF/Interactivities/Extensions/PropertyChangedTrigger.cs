namespace Core.Libraries.WPF.Interactivities.Extensions
{
    public class PropertyChangedTrigger : TriggerBase<DependencyObject>
    {
        public object Binding
        {
            get { return (object)this.GetValue(BindingProperty); }
            set { this.SetValue(BindingProperty, value); }
        }
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(nameof(Binding), typeof(object), 
            typeof(PropertyChangedTrigger), new PropertyMetadata(OnBindingChanged));

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
