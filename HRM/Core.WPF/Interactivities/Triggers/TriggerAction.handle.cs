namespace Core.WPF.Interactivities
{
    public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
    {
        protected TriggerAction() : base(typeof(T)) { }

        protected new T AssociatedObject { get { return (T)base.AssociatedObject; } }

        protected sealed override Type AssociatedObjectTypeConstraint
        {
            get { return base.AssociatedObjectTypeConstraint; }
        }
    }
}
