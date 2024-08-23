namespace Core.WPF.Interactivities
{
    public abstract class TriggerBase<T> : TriggerBase where T : DependencyObject
    {
        protected TriggerBase() : base(typeof(T)) { }

        protected new T AssociatedObject { get { return (T)base.AssociatedObject; } }

        protected sealed override Type AssociatedObjectTypeConstraint { get { return base.AssociatedObjectTypeConstraint; } }
    }
}
