namespace Core.Libraries.WPF.Interactivities
{

    public sealed class BehaviorCollection : AttachableCollection<Behavior>
    {
        internal BehaviorCollection() { }

        protected override void OnAttached()
        {
            foreach (Behavior behavior in this)
            {
                behavior.Attach(AssociatedObject);
            }
        }

        protected override void OnDetaching()
        {
            foreach (Behavior behavior in this)
            {
                behavior.Detach();
            }
        }

        internal override void ItemAdded(Behavior item)
        {
            if (AssociatedObject != null)
            {
                item.Attach(AssociatedObject);
            }
        }

        internal override void ItemRemoved(Behavior item)
        {
            if (((IBehavior)item).AssociatedObject != null)
            {
                item.Detach();
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BehaviorCollection();
        }
    }
}
