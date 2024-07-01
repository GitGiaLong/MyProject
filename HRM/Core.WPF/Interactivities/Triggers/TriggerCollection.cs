using System.Windows;

namespace Core.WPF.Interactivities
{
    public sealed class TriggerCollection : AttachableCollection<TriggerBase>
    {
        public TriggerCollection() { }

        protected override void OnAttached()
        {
            foreach (TriggerBase trigger in this)
            {
                trigger.Attach(AssociatedObject);
            }
        }

        protected override void OnDetaching()
        {
            foreach (TriggerBase trigger in this)
            {
                trigger.Detach();
            }
        }

        internal override void ItemAdded(TriggerBase item)
        {
            if (AssociatedObject != null)
            {
                item.Attach(AssociatedObject);
            }
        }

        internal override void ItemRemoved(TriggerBase item)
        {
            if (((IBehavior)item).AssociatedObject != null)
            {
                item.Detach();
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TriggerCollection();
        }
    }
}
